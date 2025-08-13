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
    // ✅ 1 load logs from dispenser name
    selectedDispenserName: string = '';
    loadLogsByDispenserName() {
        this.http.get<StationRecord[]>(`${environment.serverURI}/dispenser/station/${this.id}`,this.options)
            .subscribe(res => {
                this.dislist = res.body ?? [];
                this.dispenserName = this.dislist.map(d => d.name);
                console.log("Dispenser Names: ", this.dispenserName);
            });
    }
    loaddispenserhttp(page: number): void {
        if (this.selectedDispenserName) {
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
    // ✅ 2 load logs from fuel name
    selectedFuelName: string = '';
    fuelNames: string[] = [];
    loadLogsByFuelName() {
        this.http.get<FuelRecord[]>(`${environment.serverURI}/fuels`,this.options)
            .subscribe(res => {
                this.fuelList = res.body ?? [];
                this.fuelNames = this.fuelList.map(d => d.shortName);
                console.log("fuel Name: ", this.fuelNames);
            });
    }
    loadfuelhttp(page: number): void {
        if (this.selectedFuelName) {
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
    // ✅ 3 load logs from log type
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
    // ✅ 4 load logs from period of time
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
    // ✅ 5 load logs from price
    startPrice: string = '';
    endPrice: string = '';
    loadLogsByPricehttp(page: number): void {
        console.log(this.startPrice, " - ", this.endPrice);
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/price/${this.id}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load log data by price: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }
    // ✅ 6 load logs from total liter
    startLiter: string = '';
    endLiter: string = '';
    loadLogsByTotalLitershttp(page: number): void {
        console.log(this.startLiter, " - ", this.endLiter);
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/total/liter/${this.id}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load log data by total liter: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }
    // ✅ 7 load logs from total amount
    startAmount: string = '';
    endAmount: string = '';
    loadLogsByAmounthttp(page: number): void {
        console.log(this.startAmount, " - ", this.endAmount);
        this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/total/amount/${this.id}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
            .subscribe(res => {
                this.pagedList = res.body?.data ?? [];
                console.log("load log data by total amount: ", res.body);
                this.totalItems = res.body?.totalItems ?? 0;
                this.page = res.body?.page ?? 1;
                this.pageSize = res.body?.pageSize ?? 10;
                this.totalCount = res.body?.totalItems ?? 0;
                this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
            });
    }

    // ✅ Load 2 condition filter
    LoadDispenserTwoConditionsFilter(page: number) {
        // Check if both dispenser and log are selected
        if (this.selectedDispenserName && this.selectedLogType) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispener/log/${this.id}/${this.selectedDispenserName}/${this.selectedLogType?.logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (dispenser, logtype) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        // Check if both dispenser and fuel are selected
        if (this.selectedDispenserName && this.selectedFuelName.trim()) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispener/fuel/${this.id}/${this.selectedDispenserName}/${this.selectedFuelName?.trim()}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (dispenser, fuel) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both dispenser and period are selected
        if (this.selectedDispenserName && this.startDate && this.endDate) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/dispenser/${this.id}/${this.startDate}/${this.endDate}/${this.selectedDispenserName}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (dispenser, period) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
        }
        //  Check if both dispenser and price are selected
        if (this.selectedDispenserName && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispener/price/${this.id}/${this.selectedDispenserName}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (dispenser, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both dispenser and total amount are selected
        if (this.selectedDispenserName && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispener/amount/${this.id}/${this.selectedDispenserName}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (dispenser, total amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both dispenser and total liter are selected
        if (this.selectedDispenserName && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispener/liter/${this.id}/${this.selectedDispenserName}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (dispenser, total liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
    }
    LoadFuelTwoConditionsFilter(page: number) {
        // Check if both fuel and log are selected
        if (this.selectedLogType && this.selectedFuelName) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/log/${this.id}/${this.selectedFuelName?.trim()}/${this.selectedLogType?.logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (fuel, log type) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        //  Check if both fuel and period are selected
        if (this.selectedFuelName && this.startDate && this.endDate) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/fuel/${this.id}/${this.startDate}/${this.endDate}/${this.selectedFuelName.trim()}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (fuel, period) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
        }
        //  Check if both fuel and price are selected
        if (this.selectedFuelName && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/price/${this.id}/${this.selectedFuelName.trim()}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (fuel, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both fuel and total amount are selected
        if (this.selectedFuelName && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/amount/${this.id}/${this.selectedFuelName.trim()}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (fuel, total amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both fuel and total liter are selected
        if (this.selectedFuelName && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/liter/${this.id}/${this.selectedFuelName.trim()}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (fuel, total liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
    }
    LoadLogTypeTwoConditionsFilter(page: number) {
        //  Check if both type and period are selected
        if (this.selectedLogType && this.startDate && this.endDate) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/log/${this.id}/${this.startDate}/${this.endDate}/${this.selectedLogType?.logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (log type, period) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
        }
        //  Check if both type and price are selected
        if (this.selectedLogType && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/type/price/${this.id}/${this.selectedLogType?.logType}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (log type, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both type and total amount are selected
        if (this.selectedLogType && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/type/amount/${this.id}/${this.selectedLogType?.logType}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (log type, total amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both type and total liter are selected
        if (this.selectedLogType && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/type/liter/${this.id}/${this.selectedLogType?.logType}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (log type, total liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
    }
    LoadPeriodTwoConditionsFilter(page: number) {
        //  Check if both period and price are selected
        if (this.startDate && this.endDate && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/price/${this.id}/${this.startDate}/${this.endDate}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (period, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both period and total amount are selected
        if (this.startDate && this.endDate && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/amount/${this.id}/${this.startDate}/${this.endDate}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (period, total amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both period and total liter are selected
        if (this.startDate && this.endDate && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/liter/${this.id}/${this.startDate}/${this.endDate}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (period, total liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
    }
    LoadPriceTwoConditionsFilter(page: number) {
        //  Check if both price and total amount are selected
        if (this.startPrice && this.endPrice && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/price/amount/${this.id}/${this.startAmount}/${this.endAmount}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (price, total amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both price and total liter are selected
        if (this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/price/liter/${this.id}/${this.startLiter}/${this.endLiter}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (price, total liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
        //  Check if both total liter and total amount are selected
        if (this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            console.log("url", `/log/liter/amount/${this.id}/${this.startAmount}/${this.endAmount}/${this.startLiter}/${this.endLiter}`);
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/liter/amount/${this.id}/${this.startAmount}/${this.endAmount}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load (total liter, total amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                });
            return;
        }
    }

    // ✅ Load 3 condition filter
    LoadPeriodThreeConditions(page: number) {
       if (this.startDate && this.endDate && this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/price/liter/${this.id}/${this.startDate}/${this.endDate}/${this.startPrice}/${this.endPrice}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (period, price, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.startDate && this.endDate && this.startPrice && this.endPrice && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/price/amount/${this.id}/${this.startDate}/${this.endDate}/${this.startPrice}/${this.endPrice}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (period, price, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.startDate && this.endDate && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/amount/liter/${this.id}/${this.startDate}/${this.endDate}/${this.startLiter}/${this.endLiter}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (period, amount, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.startPrice && this.endPrice && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/price/amount/liter/${this.id}/${this.startPrice}/${this.endPrice}/${this.startAmount}/${this.endAmount}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (price, amount, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
    }

    LoadDispenserThreeConditionsFilter(page: number) {
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.selectedLogType?.logType) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/fuel/log/${this.id}/${encodeURIComponent(this.selectedDispenserName)}/${encodeURIComponent(this.selectedFuelName?.trim())}/${this.selectedLogType?.logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, fuel, log) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.selectedLogType?.logType && this.startDate && this.endDate) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/dispenser/log/${this.id}/${this.startDate}/${this.endDate}/${encodeURIComponent(this.selectedDispenserName)}/${this.selectedLogType?.logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (period, dispenser, log) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startDate && this.endDate) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/dispenser/fuel/${this.id}/${this.startDate}/${this.endDate}/${encodeURIComponent(this.selectedDispenserName)}/${this.selectedFuelName.trim()}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (period, dispenser, fuel) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/fuel/price/${this.id}/${this.selectedDispenserName}/${this.selectedFuelName.trim()}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, fuel, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/fuel/amount/${this.id}/${this.selectedDispenserName}/${this.selectedFuelName.trim()}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, fuel, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/fuel/liter/${this.id}/${this.selectedDispenserName}/${this.selectedFuelName.trim()}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, fuel, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.selectedLogType?.logType && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/log/price/${this.id}/${this.selectedDispenserName}/${this.selectedLogType?.logType}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, log, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.selectedLogType?.logType && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/log/amount/${this.id}/${this.selectedDispenserName}/${this.selectedLogType?.logType}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, log, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.selectedLogType?.logType && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/log/liter/${this.id}/${this.selectedDispenserName}/${this.selectedLogType?.logType}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, log, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.startDate && this.endDate && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/dispenser/price/${this.id}/${this.selectedDispenserName}/${this.startDate}/${this.endDate}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, period, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.startDate && this.endDate && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/dispenser/amount/${this.id}/${this.selectedDispenserName}/${this.startDate}/${this.endDate}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, period, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.startDate && this.endDate && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/dispenser/liter/${this.id}/${this.selectedDispenserName}/${this.startDate}/${this.endDate}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, period, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.startAmount && this.endAmount && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/price/amount/${this.id}/${this.selectedDispenserName}/${this.startPrice}/${this.endPrice}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, price, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/price/liter/${this.id}/${this.selectedDispenserName}/${this.startPrice}/${this.endPrice}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, price, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedDispenserName && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/dispenser/liter/amount/${this.id}/${this.selectedDispenserName}/${this.startLiter}/${this.endLiter}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (dispenser, amount, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
    }
    LoadFuelThreeConditionsFilter(page: number) {
        if (this.selectedFuelName?.trim() && this.selectedLogType?.logType && this.startDate && this.endDate) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/fuel/log/${this.id}/${this.startDate}/${this.endDate}/${encodeURIComponent(this.selectedFuelName?.trim())}/${this.selectedLogType?.logType}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (period, fuel, log) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedFuelName.trim() && this.selectedLogType?.logType && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/logType/price/${this.id}/${encodeURIComponent(this.selectedFuelName.trim())}/${this.selectedLogType?.logType}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, log, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedFuelName.trim() && this.selectedLogType?.logType && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/logType/amount/${this.id}/${encodeURIComponent(this.selectedFuelName.trim())}/${this.selectedLogType?.logType}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, log, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedFuelName.trim() && this.selectedLogType?.logType && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/logType/liter/${this.id}/${encodeURIComponent(this.selectedFuelName.trim())}/${this.selectedLogType?.logType}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, log, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedFuelName.trim() && this.startPrice && this.endPrice && this.startAmount && this.startAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/price/amount/${this.id}/${this.selectedFuelName.trim()}/${this.startPrice}/${this.endPrice}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, price, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedFuelName.trim() && this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/price/liter/${this.id}/${this.selectedFuelName.trim()}/${this.startPrice}/${this.endPrice}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, price, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedFuelName.trim() && this.startAmount && this.endAmount && this.startLiter && this.endLiter) { 
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/fuel/amount/liter/${this.id}/${this.selectedFuelName.trim()}/${this.startLiter}/${this.endLiter}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, amount, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        } 
        if (this.selectedFuelName.trim() && this.startDate && this.endDate && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/fuel/price/${this.id}/${this.selectedFuelName.trim()}/${this.startDate}/${this.endDate}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, period, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedFuelName.trim() && this.startDate && this.endDate && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/fuel/amount/${this.id}/${this.selectedFuelName.trim()}/${this.startDate}/${this.endDate}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, period, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedFuelName.trim() && this.startDate && this.endDate && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/fuel/liter/${this.id}/${this.selectedFuelName.trim()}/${this.startDate}/${this.endDate}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (fuel, period, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
    }
    LoadLogThreeConditionsFilter(page: number){
         if (this.selectedLogType?.logType && this.startDate && this.endDate && this.startPrice && this.endPrice) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/log/price/${this.id}/${this.startDate}/${this.endDate}/${this.selectedLogType?.logType}/${this.startPrice}/${this.endPrice}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (log, period, price) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedLogType?.logType && this.startDate && this.endDate && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/log/amount/${this.id}/${this.startDate}/${this.endDate}/${this.selectedLogType?.logType}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (log, period, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedLogType?.logType && this.startDate && this.endDate && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/period/log/liter/${this.id}/${this.startDate}/${this.endDate}/${this.selectedLogType?.logType}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (log, period, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedLogType?.logType && this.startPrice && this.endPrice && this.startAmount && this.endAmount) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/logType/price/amount/${this.id}/${this.selectedLogType?.logType}/${this.startPrice}/${this.endPrice}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (log, price, amount) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedLogType?.logType && this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/logType/price/liter/${this.id}/${this.selectedLogType?.logType}/${this.startPrice}/${this.endPrice}/${this.startLiter}/${this.endLiter}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (logType, price, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
        }
        if (this.selectedLogType?.logType && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            console.log("url: ", `/log/logType/amount/liter/${this.id}/${this.selectedLogType?.logType}/${this.startLiter}/${this.endLiter}/${this.startAmount}/${this.endAmount}`);
            this.http.get<PagedResult<LogRecord>>(environment.serverURI + `/log/logType/amount/liter/${this.id}/${this.selectedLogType?.logType}/${this.startLiter}/${this.endLiter}/${this.startAmount}/${this.endAmount}?page=${page}&pageSize=${this.pageSize}`, this.options)
                .subscribe(res => {
                    this.pagedList = res.body?.data ?? [];
                    console.log("load 3 conditions (logType, amount, liter) data: ", res.body);
                    this.totalItems = res.body?.totalItems ?? 0;
                    this.page = res.body?.page ?? 1;
                    this.pageSize = res.body?.pageSize ?? 10;
                    this.totalCount = res.body?.totalItems ?? 0;
                    this.pages = Array.from({ length: res.body?.totalPages ?? 0 }, (_, i) => i + 1);
                }); return;
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
        // check if all conditions are selected
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.selectedLogType?.logType && this.startDate && this.endDate) {
            this.LoadDispenserThreeConditionsFilter(newPage);
            return;
        }
        // check if 3 conditions are selected
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.selectedLogType?.logType) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.selectedLogType?.logType && this.startDate && this.endDate) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startDate && this.endDate) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startPrice && this.endPrice) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startAmount && this.endAmount) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.selectedFuelName?.trim() && this.startLiter && this.endLiter) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.selectedLogType?.logType && this.startPrice && this.endPrice) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.selectedLogType?.logType && this.startAmount && this.endAmount) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.selectedLogType?.logType && this.startLiter && this.endLiter) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.startDate && this.endDate && this.startPrice && this.endPrice) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.startDate && this.endDate && this.startAmount && this.endAmount) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.startDate && this.endDate && this.startLiter && this.endLiter) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.startAmount && this.endAmount && this.startPrice && this.endPrice) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        if (this.selectedDispenserName && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            this.LoadDispenserThreeConditionsFilter(newPage); return;
        }
        //===================================================================
        if (this.selectedFuelName.trim() && this.selectedLogType?.logType && this.startPrice && this.endPrice) {
            this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        if (this.selectedFuelName.trim() && this.selectedLogType?.logType && this.startAmount && this.endAmount) {
           this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        if (this.selectedFuelName.trim() && this.selectedLogType?.logType && this.startLiter && this.endLiter) {
            this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        if (this.selectedFuelName.trim() && this.startPrice && this.endPrice && this.startAmount && this.startAmount) {
            this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        if (this.selectedFuelName.trim() && this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        if (this.selectedFuelName.trim() && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        if (this.selectedFuelName.trim() && this.startDate && this.endDate && this.startPrice && this.endPrice) {
           this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        if (this.selectedFuelName.trim() && this.startDate && this.endDate && this.startAmount && this.endAmount) {
           this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        if (this.selectedFuelName.trim() && this.startDate && this.endDate && this.startLiter && this.endLiter) {
           this.LoadFuelThreeConditionsFilter(newPage); return;
        }
        //===================================================================
        if (this.selectedLogType?.logType && this.startDate && this.endDate && this.startPrice && this.endPrice) {
            this.LoadLogThreeConditionsFilter(newPage); return;
        }
        if (this.selectedLogType?.logType && this.startDate && this.endDate && this.startAmount && this.endAmount) {
            this.LoadLogThreeConditionsFilter(newPage); return;
        }
        if (this.selectedLogType?.logType && this.startDate && this.endDate && this.startLiter && this.endLiter) {
            this.LoadLogThreeConditionsFilter(newPage); return;
        }
        if (this.selectedLogType?.logType && this.startPrice && this.endPrice && this.startAmount && this.startAmount) {
            this.LoadLogThreeConditionsFilter(newPage); return;
        }
        if (this.selectedLogType?.logType && this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.LoadLogThreeConditionsFilter(newPage); return;
        }
        if (this.selectedLogType?.logType && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
           this.LoadLogThreeConditionsFilter(newPage); return;
        }
        //===================================================================
         if (this.startDate && this.endDate && this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.LoadPeriodThreeConditions(newPage); return;
        }
        if (this.startDate && this.endDate && this.startPrice && this.endPrice && this.startAmount && this.endAmount) {
            this.LoadPeriodThreeConditions(newPage); return;
        }
        if (this.startDate && this.endDate && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            this.LoadPeriodThreeConditions(newPage); return;
        }
        if (this.startPrice && this.endPrice && this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            this.LoadPeriodThreeConditions(newPage); return;
        }
        // check if 2 conditions are selected
        else if (this.selectedDispenserName && this.selectedFuelName.trim()) {
            this.LoadDispenserTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.selectedLogType) {
            this.LoadDispenserTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.startDate && this.endDate) {
            this.LoadDispenserTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.startPrice && this.endPrice) {
            this.LoadDispenserTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.startAmount && this.endAmount) {
            this.LoadDispenserTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedDispenserName && this.startLiter && this.endLiter) {
            this.LoadDispenserTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedFuelName?.trim() && this.selectedLogType) {
            this.LoadFuelTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedFuelName?.trim() && this.startDate && this.endDate) {
            this.LoadFuelTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedFuelName?.trim() && this.startPrice && this.endPrice) {
            this.LoadFuelTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedFuelName?.trim() && this.startAmount && this.endAmount) {
            this.LoadFuelTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedFuelName?.trim() && this.startLiter && this.endLiter) {
            this.LoadFuelTwoConditionsFilter(newPage);
            return;
        }
        //===================================================================
        else if (this.selectedLogType && this.startDate && this.endDate) {
            this.LoadLogTypeTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedLogType && this.startPrice && this.endPrice) {
            this.LoadLogTypeTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedLogType && this.startAmount && this.endAmount) {
            this.LoadLogTypeTwoConditionsFilter(newPage);
            return;
        }
        else if (this.selectedLogType && this.startLiter && this.endLiter) {
            this.LoadLogTypeTwoConditionsFilter(newPage);
            return;
        }
        //===================================================================
        else if (this.startDate && this.endDate && this.startPrice && this.endPrice) {
            this.LoadPeriodTwoConditionsFilter(newPage);
            return;
        }
        else if (this.startDate && this.endDate && this.startAmount && this.endAmount) {
            this.LoadPeriodTwoConditionsFilter(newPage);
            return;
        }
        else if (this.startDate && this.endDate && this.startLiter && this.endLiter) {
            this.LoadPeriodTwoConditionsFilter(newPage);
            return;
        }
        //===================================================================
        else if (this.startAmount && this.endAmount && this.startPrice && this.endPrice) {
            this.LoadPriceTwoConditionsFilter(newPage);
            return;
        }
        else if (this.startPrice && this.endPrice && this.startLiter && this.endLiter) {
            this.LoadPriceTwoConditionsFilter(newPage);
            return;
        }
        else if (this.startAmount && this.endAmount && this.startLiter && this.endLiter) {
            this.LoadPriceTwoConditionsFilter(newPage);
            return;
        }
        // check if only one condition is selected
        else if (this.selectedDispenserName) {
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
        else if (this.startPrice && this.endPrice) {
            this.loadLogsByPricehttp(newPage);
            return;
        }
        else if (this.startLiter && this.endLiter) {
            this.loadLogsByTotalLitershttp(newPage);
            return;
        }
        else if (this.startAmount && this.endAmount) {
            this.loadLogsByAmounthttp(newPage);
            return;
        }
        else {
            this.loadLogshttp(newPage);
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
            this.startAmount='';
            this.endAmount='';
            this.startLiter='';
            this.endLiter='';
        }
    }

    ngOnChanges(): void { }
}
