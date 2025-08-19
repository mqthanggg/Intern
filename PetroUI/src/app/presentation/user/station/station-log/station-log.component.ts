import { Component, Input, OnChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../../environments/environment';
import { FuelRecord, LogRecord, PagedResult, StationRecord } from './station-log-record';
import { TitleService } from '../../../../infrastructure/services/title.service';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxPaginationModule } from "ngx-pagination";

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
    pages: number[] = [];
    stationName: string | undefined;
    options = {
        observe: 'response' as const,
        withCredentials: true
    };
    constructor(private titleService: TitleService, private http: HttpClient, private route: ActivatedRoute, private router: Router) { }
    items = [];
    pagedList: LogRecord[] = [];
    totalCount: number = 0;
    pageSize: number = 20;
    totalItems: number = 1;
    pagedLogs: PagedResult<LogRecord> | undefined;

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

    dislist: StationRecord[] = [];
    dispenserName: string[] = [];
    loadLogsByDispenserName() {
        this.http.get<StationRecord[]>(`${environment.serverURI}/dispenser/station/${this.id}`, this.options)
            .subscribe(res => {
                this.dislist = res.body ?? [];
                this.dispenserName = this.dislist.map(d => d.name);
                console.log("Dispenser Names: ", this.dispenserName);
            });
    }

    fuelList: FuelRecord[] = [];
    fuelNames: string[] = [];
    loadLogsByFuelName() {
        this.http.get<FuelRecord[]>(`${environment.serverURI}/fuels`, this.options)
            .subscribe(res => {
                this.fuelList = res.body ?? [];
                this.fuelNames = this.fuelList.map(d => d.shortName);
                console.log("fuel Name: ", this.fuelNames);
            });
    }

    logTypeList: { logType: number; logTypeName: string }[] = [
        { logType: 1, logTypeName: 'Bán lẻ' },
        { logType: 2, logTypeName: 'Công nợ' },
        { logType: 3, logTypeName: 'Khuyến mãi' },
        { logType: 4, logTypeName: 'Trả trước' }
    ];

    selectedLogType: { logType: number; logTypeName: string } | null = null;
    selectedDispenserName: string = '';
    selectedFuelName: string = '';
    startDate: string = '';
    endDate: string = '';
    startPrice: string = '';
    endPrice: string = '';
    startLiter: string = '';
    endLiter: string = '';
    startAmount: string = '';
    endAmount: string = '';

    onPageChange(newPage: number) {
        this.loadLogsByFullFilter(newPage);
    }
    loadLogsByFullFilter(page: number): void {
        const filter: any = {
            stationId: this.id ?? null,
            name: this.selectedDispenserName || null,
            fuelName: this.selectedFuelName?.trim() || null,
            logType: this.selectedLogType?.logType || null,
            fromDate: this.startDate ? new Date(this.startDate).toISOString() : null,
            toDate: this.endDate ? new Date(this.endDate).toISOString() : null,
            fromPrice: this.startPrice || null,
            toPrice: this.endPrice || null,
            fromAmount: this.startAmount || null,
            toAmount: this.endAmount || null,
            fromLiter: this.startLiter || null,
            toLiter: this.endLiter || null
        };
        let apiUrl: string;
        if (this.startDate && this.endDate) {
            apiUrl = environment.serverURI + `/get/fulltime/filter?page=${page}&pageSize=${this.pageSize}`;
        } else {
            apiUrl = environment.serverURI + `/get/full/filter?page=${page}&pageSize=${this.pageSize}`;
        }
        this.http.post<PagedResult<LogRecord>>(apiUrl, filter, this.options)
            .subscribe({
                next: (res) => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("Loaded data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? this.pageSize;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                },
                error: (err) => {
                    console.error("Error loading data", err);
                },
            });
    }
    
    ngOnInit(): void {
        this.loadLogsByDispenserName();
        this.loadLogsByFuelName();
        this.route.paramMap.subscribe(params => {
            const page = +params.get('page')!;
            this.loadLogsByFullFilter(page);
        });
        this.http.get<StationRecord>(`${environment.serverURI}/station/${this.id}`, this.options).subscribe(
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

    // ✅ Reset filter
    clearFilters() {
        this.clearFilters = () => {
            this.selectedDispenserName = '';
            this.selectedFuelName = '';
            this.selectedLogType = null;
            this.startDate = '';
            this.endDate = '';
            this.startAmount = '';
            this.endAmount = '';
            this.startLiter = '';
            this.endLiter = '';
            this.loadLogsByFullFilter(1);
        }
    }
    ngOnChanges(): void { }
}
