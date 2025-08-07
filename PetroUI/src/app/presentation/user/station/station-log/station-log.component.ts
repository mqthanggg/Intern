import { Component, Input, OnChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../../environments/environment';
import { LogRecord, PagedResult, Station } from './station-log-record';
import { TitleService } from '../../../../infrastructure/services/title.service';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxPaginationModule, PaginationInstance } from "ngx-pagination";

@Component({
    selector: 'app-report-station-chart',
    standalone: true,
    imports: [NgChartsModule, ReactiveFormsModule, CommonModule, FormsModule, NgxPaginationModule],
    providers: [],
    templateUrl: './station-log.component.html',
    //   styleUrl: './station-log.component.css'
})

export class StationLogComponent implements OnChanges {
    @Input() id: number = -1;
    page: number = 1;
    isUpdateLoading = false;
    pages: number[] = [];
    stationName: string | undefined;
    options = {
        observe: 'response' as const,
        withCredentials: false
    };
    constructor(private titleService: TitleService, private http: HttpClient, private route: ActivatedRoute, private router: Router) { }

    // ✅ Load table log 
    logsocket: WebSocketSubject<LogRecord[]> | undefined
    logList: LogRecord[] = [];
    pagesocket: WebSocketSubject<PagedResult<LogRecord>> | undefined;

    // ✅ pagination
    items = [];
    pagedList: LogRecord[] = [];
    totalCount: number = 0;
    pageSize: number = 20;
    totalItems: number = 1;
    pagedLogs: PagedResult<LogRecord> | undefined;
    loadLogshttp(page: number): void {
        page= page || 1;
        this.http.get<PagedResult<LogRecord>>(`${environment.serverURI}/pagelog/station/${this.id}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load log data: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });

    }

    onPageChange(newPage: number) {
        this.loadLogshttp(newPage);
    }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const page = +params.get('page')!;
            this.loadLogshttp(page);
        });
        this.http.get<Station>(`${environment.serverURI}/station/${this.id}`, this.options).subscribe(
            {
                next: (res) => {
                    console.log("data: ", res);
                    console.log('StationName:', res.body?.name);
                    this.stationName = res.body?.name;
                    this.titleService.updateTitle(this.stationName || 'Station Name');
                },
                error: (error) => {
                    console.error('Error:', error);
                }
            }
        );
    }

    // // ✅ Reset filter
    // clearFilters() {
    //     this.filter = { fromDate: '', toDate: '' };
    //     this.filteredList = [...this.logList];
    //     this.selectedTimeFilter = '';
    // }

    // // ✅ filter
    // selectedTimeFilter: string = '';
    // onTimeFilterChange() {
    //     if (this.selectedTimeFilter !== 'range') {
    //         this.filter.fromDate = '';
    //         this.filter.toDate = '';
    //     }
    // }
    getLogTypeClass(type: string): string {
        switch (type) {
            case 'Bán lẻ':
                return 'bg-blue-400 text-white w-30';
            case 'Công nợ':
                return 'bg-yellow-400 text-white w-30';
            case 'Khuyến mãi':
                return 'bg-green-400 text-white w-30';
            case 'Trả trước':
                return 'bg-purple-400 text-white w-30';
            default:
                return 'bg-gray-400 text-white w-30';
        }
    }

    // // ✅ filter by period of time
    // filter = {
    //     fromDate: '',
    //     toDate: ''
    // };
    // filteredList: LogRecord[] = [];
    // applyDateFilter() {
    //     const from = this.filter.fromDate ? new Date(this.filter.fromDate) : null;
    //     const to = this.filter.toDate ? new Date(this.filter.toDate) : null;
    //     this.filteredList = this.logList.filter(item => {
    //         const itemDate = new Date(item.time);
    //         if (from && itemDate < from) return false;
    //         if (to && itemDate > to) return false;
    //         return true;
    //     });
    // }

    // // ✅ filter by log type
    // selectedLogType: string = '';
    // logTypeList: string[] = [];
    // applyLogTypeFilter() {
    //     let filtered = [...this.logList];
    //     // Fitter by LogName
    //     if (this.selectedLogType) {
    //         filtered = filtered.filter(item => item.logTypeName === this.selectedLogType);
    //     }
    //     this.filteredList = filtered;
    // }

    // // ✅ filter by fuel name
    // fuelFilter: string = '';
    // fuelList: string[] = [];
    // filterByFuel() {
    //     if (this.fuelFilter) {
    //         this.filteredList = this.logList.filter(item =>
    //             item.fuelName.toLowerCase().includes(this.fuelFilter.toLowerCase())
    //         );
    //     } else {
    //         this.filteredList = this.logList;
    //     }
    // }

    // //  ✅ filter by date
    // selectedDate: string = '';
    // filterBySingleDate() {
    //     if (!this.selectedDate) {
    //         this.filteredList = this.logList;
    //     } else {
    //         const selected = new Date(this.selectedDate);
    //         this.filteredList = this.logList.filter(item => {
    //             const itemDate = new Date(item.time);
    //             return itemDate.getFullYear() === selected.getFullYear() &&
    //                 itemDate.getMonth() === selected.getMonth() &&
    //                 itemDate.getDate() === selected.getDate();
    //         });
    //     }
    // }

    // // ✅ filter by dispenser pump
    // selectedPump: number | '' = '';
    // pumpList: number[] = [];
    // filterByPump() {
    //     let filtered = [...this.pagedList];
    //     if (this.selectedPump !== '') {
    //         filtered = filtered.filter(item => item.name === this.selectedPump);
    //     }
    //     this.filteredList = filtered;
    // }


    ngOnChanges(): void {
        this.logsocket?.complete();
    }
}
