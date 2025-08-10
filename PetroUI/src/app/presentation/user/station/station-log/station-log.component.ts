import { Component, Input, OnChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WebSocketSubject } from 'rxjs/webSocket';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../../environments/environment';
import { FuelRecord, LogRecord, PagedResult, StationRecord } from './station-log-record';
import { TitleService } from '../../../../infrastructure/services/title.service';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxPaginationModule } from "ngx-pagination";
import { forkJoin } from 'rxjs';

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
        withCredentials: false
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

    // ✅ Load dropdown options
    dislist: StationRecord[] = [];
    dispenserName: string[] = [];
    loadLogsByDispenserName() {
        this.http.get<StationRecord[]>(`${environment.serverURI}/dispenser/station/${this.id}`)
            .subscribe(res => {
                this.dislist = res;
                this.dispenserName = this.dislist.map(d => d.name);
                console.log("Dispenser Names: ", this.dispenserName);
            });
    }

    fuelList: FuelRecord[] = [];
    fuelNames: string[] = [];
    loadLogsByFuelName() {
        this.http.get<FuelRecord[]>(`${environment.serverURI}/fuels`)
            .subscribe(res => {
                this.fuelList = res;
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

    // ✅ load logs from log type
    selectedLogType: { logType: number; logTypeName: string } | null = null;
    loadLogshttp(page: number): void {
        page = page || 1;
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/pagelog/station/${this.id}?page=${page}&pageSize=${this.pageSize}`, this.options)
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
    selectedDispenserName: string = '';
    loaddispenserhttp(page: number): void {
        if(this.selectedDispenserName){
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/${this.id}/${encodeURIComponent(this.selectedDispenserName)}?page=${page}&pageSize=${this.pageSize}`, this.options)
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
        console.log("Selected dispenser name: 0");
    }

    // ✅ load logs from fuel name
    selectedFuelName: string = '';
    loadfuelhttp(page: number): void {
       if(this.selectedFuelName){
         this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/${this.id}/${encodeURIComponent(this.selectedFuelName?.trim())}?page=${page}&pageSize=${this.pageSize}`, this.options)
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
       console.log("Selected fuel name: 0");
    }

    // ✅ load logs from log type
    loadtypehttp(page: number): void {
        const logType = this.selectedLogType?.logType;
        if (logType) {
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
        console.log("Selected log type: 0");
    }

    // ✅ load logs from period of time
    startDate: string = '';
    endDate: string = '';
    loadLogsByPeriodhttp(page: number): void {
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

    // ✅ Load 2 condition filter
    LoadTwoConditionsFilter(page: number) {
        const logType = this.selectedLogType?.logType;
        console.log("Selected log type: ", logType);
        // Check if both dispenser and fuel are selected
        if (this.selectedDispenserName && this.selectedFuelName.trim()) {
            const dispenserRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/dispenser/${this.id}/${encodeURIComponent(this.selectedDispenserName)}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            const fuelRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/fuel/${this.id}/${encodeURIComponent(this.selectedFuelName.trim())}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            forkJoin([dispenserRequest, fuelRequest]).subscribe({
                next: ([dispenserRes, fuelRes]) => {
                    const dispenserData = dispenserRes.body?.data ?? dispenserRes.body?.data ?? [];
                    const fuelData = fuelRes.body?.data ?? fuelRes.body?.data ?? [];
                    const intersected = dispenserData.filter(item =>
                        fuelData.some(fuelItem => fuelItem.fuelName === item.fuelName)
                    );
                    this.pagedList = intersected;
                    this.totalItems = intersected.length;
                    this.totalCount = intersected.length;
                    const totalPages = Math.ceil(this.totalItems / this.pageSize);
                    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
                    console.log("dispenser & fuel:", this.pagedList);
                },
                error: (err) => {
                    console.error("error data API", err);
                }
            }); return;
        }
        // Check if both dispenser and log are selected
        if (this.selectedDispenserName && this.selectedLogType) {
            const encodedDispenserName = encodeURIComponent(this.selectedDispenserName?.trim());
            const logTypeRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/type/${this.id}/${logType}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            const dispenserRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/dispenser/${this.id}/${encodedDispenserName}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            forkJoin([logTypeRequest, dispenserRequest]).subscribe(
                ([logTypeRes, dispenserRes]) => {
                    const logTypeData = logTypeRes.body?.data ?? [];
                    const dispenserData = dispenserRes.body?.data ?? [];
                    const intersected = logTypeData.filter(item =>
                        dispenserData.some(other => other.name === item.name)
                    );
                    this.pagedList = intersected;
                    this.totalItems = intersected.length;
                    this.totalCount = intersected.length;
                    this.page = page;
                    const totalPages = Math.ceil(this.totalItems / this.pageSize);
                    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
                    console.log("dispenser && logType:", this.pagedList);
                },
                error => {
                    console.error("error data API", error);
                }
            ); return;
        }
        // Check if both fuel and log are selected
        if (this.selectedLogType && this.selectedFuelName) {
            const logTypeRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/type/${this.id}/${logType}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            const fuelRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/fuel/${this.id}/${encodeURIComponent(this.selectedFuelName?.trim())}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            forkJoin([logTypeRequest, fuelRequest]).subscribe(
                ([logTypeRes, fuelRes]) => {
                    const logTypeData = logTypeRes.body?.data ?? [];
                    const fuelData = fuelRes.body?.data ?? [];
                    const intersected = logTypeData.filter(item =>
                        fuelData.some(fuelItem => fuelItem.fuelName === item.fuelName)
                    );
                    this.pagedList = intersected;
                    this.totalItems = intersected.length;
                    this.totalCount = intersected.length;
                    this.page = page;
                    const totalPages = Math.ceil(this.totalItems / this.pageSize);
                    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
                    console.log("fuel && logType:", this.pagedList);
                },
                error => {
                    console.error("error data API:", error);
                }
            ); return;
        }
        //  Check if both dispenser and period are selected
        if (this.selectedDispenserName && this.startDate && this.endDate) {
            const dispenserRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/dispenser/${this.id}/${encodeURIComponent(this.selectedDispenserName)}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            const periodRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/period/${this.id}/${this.startDate}/${this.endDate}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            forkJoin([dispenserRequest, periodRequest]).subscribe(
                ([dispenserRes, periodRes]) => {
                    const dispenserData = dispenserRes.body?.data ?? [];
                    const periodData = periodRes.body?.data ?? [];
                    const intersected = periodData.filter(item => dispenserData.some(dispenserItem => dispenserItem.name === item.name));

                    this.pagedList = intersected;
                    this.totalItems = intersected.length;
                    this.totalCount = intersected.length;
                    this.page = page;
                    const totalPages = Math.ceil(this.totalItems / this.pageSize);
                    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
                    console.log("dispenser && period:", this.pagedList);
                },
                error => {
                    console.error("error data API:", error);
                }
            ); return;
        }
        //  Check if both fuel and period are selected
        if (this.selectedFuelName && this.startDate && this.endDate) {
            const fuelRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/fuel/${this.id}/${encodeURIComponent(this.selectedFuelName?.trim())}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            const periodRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/period/${this.id}/${this.startDate}/${this.endDate}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            forkJoin([fuelRequest, periodRequest]).subscribe(
                ([fuelRes, periodRes]) => {
                    const fuelData = fuelRes.body?.data ?? [];
                    const periodData = periodRes.body?.data ?? [];
                    const intersected = periodData.filter(item => fuelData.some(Item => Item.fuelName === item.fuelName));

                    this.pagedList = intersected;
                    this.totalItems = intersected.length;
                    this.totalCount = intersected.length;
                    this.page = page;
                    const totalPages = Math.ceil(this.totalItems / this.pageSize);
                    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
                    console.log("fuel && period:", this.pagedList);
                },
                error => {
                    console.error("error data API:", error);
                }
            ); return;
        }
        //  Check if both log and period are selected
        if (this.selectedLogType && this.startDate && this.endDate) {
            const logTypeRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/type/${this.id}/${logType}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            const periodRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/period/${this.id}/${this.startDate}/${this.endDate}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            forkJoin([logTypeRequest, periodRequest]).subscribe(
                ([logTypeRes, periodRes]) => {
                    const logTypeData = logTypeRes.body?.data ?? [];
                    const periodData = periodRes.body?.data ?? [];
                    const intersected = periodData.filter(item => logTypeData.some(Item => Item.logTypeName === item.logTypeName));

                    this.pagedList = intersected;
                    this.totalItems = intersected.length;
                    this.totalCount = intersected.length;
                    this.page = page;
                    const totalPages = Math.ceil(this.totalItems / this.pageSize);
                    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
                    console.log("logType && period:", this.pagedList);
                },
                error => {
                    console.error("error data API:", error);
                }
            );
            return;
        }
    }

    // ✅ Load 3 condition filter
    LoadAllThreeConditions(page: number) {
        const logType = this.selectedLogType?.logType;
        if (this.selectedFuelName?.trim() && logType && this.startDate && this.endDate) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/fuel/log/${this.id}/${this.startDate}/${this.endDate}/${encodeURIComponent(this.selectedFuelName?.trim())}/${logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (period, fuel, log) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && logType) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/fuel/log/${this.id}/${encodeURIComponent(this.selectedDispenserName)}/${encodeURIComponent(this.selectedFuelName?.trim())}/${logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, fuel, log) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        if (this.selectedDispenserName && logType && this.startDate && this.endDate) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/dispenser/log/${this.id}/${this.startDate}/${this.endDate}/${encodeURIComponent(this.selectedDispenserName)}/${logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (period, dispenser, log) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startDate && this.endDate) {
            const dispenserRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/dispenser/${this.id}/${encodeURIComponent(this.selectedDispenserName)}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            const fuelRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/fuel/${this.id}/${encodeURIComponent(this.selectedFuelName.trim())}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            const periodRequest = this.http.get<PagedResult<LogRecord>>(
                `${environment.serverURI}/log/period/${this.id}/${this.startDate}/${this.endDate}?page=${page}&pageSize=${this.pageSize}`,
                this.options
            );
            forkJoin([dispenserRequest, fuelRequest, periodRequest]).subscribe(
                ([dispenserRes, fuelRes, periodRes]) => {
                    const dispenserData = dispenserRes.body?.data ?? [];
                    const fuelData = fuelRes.body?.data ?? [];
                    const periodData = periodRes.body?.data ?? [];
                    const intersected = dispenserData.filter(item =>
                        fuelData.some(fuel => fuel.fuelName === item.fuelName) &&
                        periodData.some(period => period.time === item.time)
                    );
                    this.pagedList = intersected;
                    this.totalItems = intersected.length;
                    this.totalCount = intersected.length;
                    this.page = page;
                    const totalPages = Math.ceil(this.totalItems / this.pageSize);
                    this.pages = Array.from({ length: totalPages }, (_, i) => i + 1);
                    console.log("period, dispenser, fuel:", this.pagedList);
                },
                error => {
                    console.error("Lỗi khi gọi API:", error);
                }
            );
            return;
        }
    }

    // ✅ Load 4 condition filter
    LoadAllConditions(page: number) {
        const logType = this.selectedLogType?.logType;
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/full/condition/${this.id}/${this.startDate}/${this.endDate}/${encodeURIComponent(this.selectedDispenserName)}/${encodeURIComponent(this.selectedFuelName?.trim())}/${logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                console.log("✅ API call success, raw response:", res);
                this.pagedList = res.body?.data ?? [];
                console.log("load all conditions data: ", res.body);

                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }

    onPageChange(newPage: number) {
        const logType = this.selectedLogType?.logType;
        // check if all conditions are selected
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && logType && this.startDate && this.endDate) {
            this.LoadAllConditions(newPage);
            return;
        }
        // check if 3 conditions are selected
        else if (this.selectedDispenserName && this.selectedFuelName?.trim() && logType) {
            this.LoadAllThreeConditions(newPage);
            return;
        }
        else if (this.selectedDispenserName && logType && this.startDate && this.endDate) {
            this.LoadAllThreeConditions(newPage);
            return;
        }
        else if (this.selectedFuelName?.trim() && logType && this.startDate && this.endDate) {
            this.LoadAllThreeConditions(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.selectedFuelName?.trim() && logType) {
            this.LoadAllThreeConditions(newPage);
            return;
        }
        else if (this.selectedDispenserName && logType && this.startDate && this.endDate) {
            this.LoadAllThreeConditions(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startDate && this.endDate) {
            this.LoadAllThreeConditions(newPage);
            return;
        }
        // check if 2 conditions are selected
        else if (this.selectedDispenserName && this.selectedFuelName.trim()) {
            this.LoadTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.selectedLogType) {
            this.LoadTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedFuelName?.trim() && this.selectedLogType) {
            this.LoadTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.startDate && this.endDate) {
            this.LoadTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedFuelName?.trim() && this.startDate && this.endDate) {
            this.LoadTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedLogType && this.startDate && this.endDate) {
            this.LoadTwoConditionsFilter(newPage);
            return;
        }
        // check if only one condition is selected
        if (this.selectedDispenserName) {
            this.loaddispenserhttp(newPage);
            return;
        }
        else if (this.selectedFuelName) {
            this.loadfuelhttp(newPage);
            return;
        }
        else if (this.selectedLogType) {
            this.loadtypehttp(newPage);
            return;
        }
        else if (this.startDate && this.endDate) {
            this.loadLogsByPeriodhttp(newPage);
            return;
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
            this.loadLogshttp(1);
        }
    }

    ngOnChanges(): void { }
}
