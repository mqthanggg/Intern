import { Component, Input, OnChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../../environments/environment';
import { LogRecord, Station } from './station-log-record';
import { TitleService } from '../../../../infrastructure/services/title.service';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'app-report-station-chart',
    standalone: true,
    imports: [NgChartsModule, ReactiveFormsModule, CommonModule, FormsModule],
    templateUrl: './station-log.component.html',
    //   styleUrl: './station-log.component.css'
})

export class StationLogComponent implements OnChanges {
    @Input() id: number = -1;
    isUpdateLoading = false;
    stationName: string | undefined;
    options = {
        observe: 'response' as const,
        withCredentials: false
    };
    
    constructor(private titleService: TitleService, private http: HttpClient, private route: ActivatedRoute) { }

    // ✅ Load table log 
    logsocket: WebSocketSubject<LogRecord[]> | undefined
    logList: LogRecord[] = [];

    // ✅ pagination
    pagedList: LogRecord[] = [];
    pageSize = 15;  // the number of row in page
    currentPage = 1;
    totalPages = 0;
    pages: number[] = [];
    updatePagedList() {
        const startIndex = (this.currentPage - 1) * this.pageSize;
        const endIndex = startIndex + this.pageSize;
        this.pagedList = this.filteredList.slice(startIndex, endIndex);
        this.totalPages = Math.ceil(this.filteredList.length / this.pageSize);
        this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
    }
    goToPage(page: number) {
        if (page < 1 || page > Math.ceil(this.filteredList.length / this.pageSize)) return;
        this.currentPage = page;
        this.updatePagedList();
    }

    // ✅ Reset filter
    clearFilters() {
        this.filter = { fromDate: '', toDate: '' };
        this.filteredList = [...this.logList];
        this.currentPage = 1;
        this.updatePagedList();
        this.selectedTimeFilter = '';
    }

    // ✅ filter
    selectedTimeFilter: string = '';
    onTimeFilterChange() {
        if (this.selectedTimeFilter !== 'range') {
            this.filter.fromDate = '';
            this.filter.toDate = '';
        }
    }
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

    // ✅ filter by period of time
    filter = {
        fromDate: '',
        toDate: ''
    };
    filteredList: LogRecord[] = [];
    applyDateFilter() {
        const from = this.filter.fromDate ? new Date(this.filter.fromDate) : null;
        const to = this.filter.toDate ? new Date(this.filter.toDate) : null;
        this.filteredList = this.logList.filter(item => {
            const itemDate = new Date(item.Time);
            if (from && itemDate < from) return false;
            if (to && itemDate > to) return false;
            return true;
        });
        this.currentPage = 1;
        this.updatePagedList();
    }

    // ✅ filter by log type
    selectedLogType: string = '';
    logTypeList: string[] = [];
    applyLogTypeFilter() {
        let filtered = [...this.logList];
        // Fitter by LogName
        if (this.selectedLogType) {
            filtered = filtered.filter(item => item.LogTypeName === this.selectedLogType);
        }
        this.filteredList = filtered;
        this.currentPage = 1;
        this.updatePagedList();
    }

    // ✅ filter by fuel name
    fuelFilter: string = '';
    fuelList: string[] = [];
    filterByFuel() {
        if (this.fuelFilter) {
            this.filteredList = this.logList.filter(item =>
                item.FuelName.toLowerCase().includes(this.fuelFilter.toLowerCase())
            );
        } else {
            this.filteredList = this.logList;
        }
        this.currentPage = 1;
        this.updatePagedList();
    }

    //  ✅ filter by date
    selectedDate: string = '';
    filterBySingleDate() {
        if (!this.selectedDate) {
            this.filteredList = this.logList;
        } else {
            const selected = new Date(this.selectedDate);
            this.filteredList = this.logList.filter(item => {
                const itemDate = new Date(item.Time);
                return itemDate.getFullYear() === selected.getFullYear() &&
                    itemDate.getMonth() === selected.getMonth() &&
                    itemDate.getDate() === selected.getDate();
            });
        }
        this.currentPage = 1;
        this.updatePagedList();
    }

    // ✅ filter by dispenser pump
    selectedPump: number | '' = '';
    pumpList: number[] = [];
    filterByPump() {
        let filtered = [...this.logList];
        if (this.selectedPump !== '') {
            filtered = filtered.filter(item => item.Name === this.selectedPump);
        }
        this.filteredList = filtered;
        this.currentPage = 1;
        this.updatePagedList();
    }

    ngOnInit(): void {
        this.http.get<Station>(`${environment.serverURI}/station/${this.id}`, this.options).subscribe(
            (res) => {
                console.log("data: ", res);
                console.log('StationName:', res.body?.name);
                this.stationName = res.body?.name;
                this.titleService.updateTitle(this.stationName || 'Station Name');
            },
            (error) => {
                console.error('Error:', error);
            }
        );

        // ✅ Load table log by StationId
        this.logsocket = webSocket<LogRecord[]>(environment.wsServerURI + `/ws/fulllog/station/${this.id}?token=${localStorage.getItem('jwt')}`);
        this.logsocket.subscribe({
            next: (res: LogRecord[]) => {
                this.logList = res;
                console.log("load log data: ", this.logList);
                this.logTypeList = Array.from(new Set(this.logList.map(i => i.LogTypeName)));
                this.filteredList = [...this.logList];
                this.fuelList = [...new Set(this.logList.map(item => item.FuelName))];
                this.pumpList = [...new Set(this.logList.map(item => item.Name))];
                if (this.filter?.fromDate || this.filter?.toDate) {
                    this.applyDateFilter();
                    this.applyLogTypeFilter();
                    this.filterByFuel();
                    this.filterByPump();
                } else {
                    this.updatePagedList();
                }
            },
            complete: () => console.log("WebSocket connection closed"),
            error: err => {
                console.error("(WebSocket error) - not load data log", err);
            }
        });
    }

    ngOnChanges(): void {
        this.logsocket?.complete();
    }
}
