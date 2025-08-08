import { Component, Input, OnChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../../environments/environment';
import { FuelRecord, LogRecord, PagedResult, StationRecord } from './station-log-record';
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

    // ✅ pagination
    logsocket: WebSocketSubject<LogRecord[]> | undefined
    logList: LogRecord[] = [];
    pagesocket: WebSocketSubject<PagedResult<LogRecord>> | undefined;
    items = [];
    pagedList: LogRecord[] = [];
    totalCount: number = 0;
    pageSize: number = 20;
    totalItems: number = 1;
    pagedLogs: PagedResult<LogRecord> | undefined;
    loadLogshttp(page: number): void {
        page = page || 1;
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

    // ✅ load logs from dispenser name
    dislist: StationRecord[] = [];
    dispenserName: string[] = [];
    selectedDispenserName: string = '';
    loadLogsByDispenserName() {
        this.http.get<StationRecord[]>(`${environment.serverURI}/dispenser/station/${this.id}`)
            .subscribe(res => {
                this.dislist = res;
                this.dispenserName = this.dislist.map(d => d.name);
                console.log("Dispenser Names: ", this.dispenserName);
            });
    }
    loaddispenserhttp(page: number): void {
        page = page || 1;
        const selectedName = this.selectedDispenserName;
        if (!selectedName) {
            console.warn("No dispenser name selected");
            return;
        }
        const encodedName = encodeURIComponent(selectedName);
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/${this.id}/${encodedName}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load dispenser data: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }

    // ✅ load logs from fuel name
    fuelList: FuelRecord[] = [];
    selectedFuelName: string = '';
    fuelNames: string[] = [];
    loadLogsByFuelName() {
        this.http.get<FuelRecord[]>(`${environment.serverURI}/fuels`)
            .subscribe(res => {
                this.fuelList = res;
                this.fuelNames = this.fuelList.map(d => d.shortName);
                console.log("fuel Name: ", this.fuelNames);
            });
    }
    loadfuelhttp(page: number): void {
        page = page || 1;
        const selectedName = this.selectedFuelName;
        if (!selectedName) {
            console.warn("No fuel name selected");
            return;
        }
        const encodedName = encodeURIComponent(selectedName);
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/${this.id}/${encodedName}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load fuel data: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }

    // ✅ load logs from log type
    logTypeList: { logType: number; logTypeName: string }[] = [
        { logType: 1, logTypeName: 'Bán lẻ' },
        { logType: 2, logTypeName: 'Công nợ' },
        { logType: 3, logTypeName: 'Khuyến mãi' },
        { logType: 4, logTypeName: 'Trả trước' }
    ];
    selectedLogType: { logType: number; logTypeName: string } | null = null;
    loadtypehttp(page: number): void {
        page = page || 1;
        const logType = this.selectedLogType?.logType;
        console.log("Selected log type: ", logType);
        if (!logType) {
            console.warn("No log type selected");
            return;
        }
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/type/${this.id}/${logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load type data: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }

    // ✅ load logs from period of time
    startDate: string = '';
    endDate: string = '';
    loadLogsByPeriodhttp(page: number): void {
        page = page || 1;
        if (!this.startDate || !this.endDate) {
            console.warn('Start date or end date is missing');
            return;
        }
        console.log(this.startDate, " - ", this.endDate);
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/${this.id}/${this.startDate}/${this.endDate}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load log data by period: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }

    // ✅ load logs by date
    selectedDate: string = '';
    loadLogsByDatehhttp(page: number): void {
        page = page || 1;
        if (!this.selectedDate) {
            console.warn('date is missing');
            return;
        }
        console.log("Selected date: ", this.selectedDate);
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/date/${this.id}/${this.selectedDate}?page=${1}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load log data by date: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }

    //==============================================
    // loadFilteredLogs(page: number): void {
    //     page = page || 1;
    //     const params: any = {
    //         page,
    //         pageSize: this.pageSize,
    //     };
    //     if (this.id) params.stationId = this.id;
    //     if (this.startDate) params.startDate = this.startDate;
    //     if (this.endDate) params.endDate = this.endDate;
    //     if (this.selectedDispenserName) params.dispenserName = this.selectedDispenserName;
    //     if (this.selectedFuelName) params.fuelName = this.selectedFuelName;
    //     if (this.selectedLogType?.logType) params.logTypeId = this.selectedLogType.logType;
    // }

    onPageChange(newPage: number) {
        if(this.selectedDispenserName){
            this.loaddispenserhttp(newPage);
        }
        else if(this.selectedFuelName){
            this.loadfuelhttp(newPage);
        }
        else if(this.selectedLogType){
            this.loadtypehttp(newPage);
        }
        else if(this.startDate || this.endDate){
            this.loadLogsByPeriodhttp(newPage);
        }
        else if(this.selectedDate){
            this.loadLogsByDatehhttp(newPage);
        }
        else
      this.loadLogshttp(newPage);
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

    ngOnInit(): void {
        this.loadLogsByDispenserName();
        this.loadLogsByFuelName();
        this.route.paramMap.subscribe(params => {
            const page = +params.get('page')!;
            this.loadLogshttp(page);
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
            this.selectedDate = '';
            this.loadLogshttp(1);
        }
    }

    ngOnChanges(): void {
        this.logsocket?.complete();
    }
}
