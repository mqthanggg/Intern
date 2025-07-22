export interface revenuestation {
    StationId: number;
    StationName: string;
    TotalRevenue: number,
    TotalLiters: number,
    TotalProfit: number;
}


export interface WSrevenuestation {
    StationId: number;
    StationName: string;
    TotalRevenue: number,
    TotalLiters: number,
    TotalProfit: number;
}

export interface revenuestationday extends revenuestation {
    Date: string;
}

export interface WSrevenuestationday extends WSrevenuestation {
    Date: string;
}

export interface revenuestationmonth extends revenuestation {
    Month: string;
}

export interface WSrevenuestationmonth extends WSrevenuestation {
    month: string;
}

export interface revenuestationyear extends revenuestation {
    Year: string;
}

export interface WSrevenuestationyear extends WSrevenuestation {
    year: string;
}
// ===============================
export interface revenuefuel {
    StationId: number;
    FuelName: string;
    TotalAmount: number;
    TotalLiters: number;
}

export interface WSrevenuefuel {
    StationId: number;
    FuelName: string;
    TotalAmount: number;
    TotalLiters: number;
}

export interface revenuefuelday extends revenuefuel {
    Date: string;
}

export interface WSrevenuefuelday extends WSrevenuefuel {
    Date: string;
}

export interface revenuefuelmonth extends revenuefuel {
    Month: string;
}

export interface WSrevenuefuelmonth extends WSrevenuefuel {
    Month: string;
}

export interface revenuefuelyear extends revenuefuel {
    Year: string;
}

export interface WSrevenuefuelyear extends WSrevenuefuel {
    Year: string;
}
// ===============================
export interface revenuetype {
    StationId: number;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
}

export interface WSrevenuetype {
    StationId: number;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
}

export interface revenuetypeday extends revenuetype {
    Date: string;
}

export interface WSrevenuetypeday extends WSrevenuetype {
    Date: string;
}

export interface revenuetypemonth extends revenuetype {
    Month: string;
}

export interface WSrevenuetypemonth extends WSrevenuetype {
    Month: string;
}

export interface revenuetypeyear extends revenuetype {
    Year: string;
}

export interface WSrevenuetypeyear extends WSrevenuetype {
    Year: string;
}