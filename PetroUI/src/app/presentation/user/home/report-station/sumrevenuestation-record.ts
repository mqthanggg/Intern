export interface revenuestation {
    StationId: number;
    StationName: string;
    TotalRevenue: number,
    TotalLiters: number,
    TotalProfit: number;
}
export interface revenuestationday extends revenuestation {
    Date: string;
}
export interface revenuestationmonth extends revenuestation {
    Month: string;
}
export interface revenuestationyear extends revenuestation {
    Year: string;
}

// ===============================
export interface revenuefuel {
    StationId: number;
    FuelName: string;
    TotalAmount: number;
    TotalLiters: number;
}
export interface revenuefuelday extends revenuefuel {
    Date: string;
}
export interface revenuefuelmonth extends revenuefuel {
    Month: string;
}
export interface revenuefuelyear extends revenuefuel {
    Year: string;
}

// ===============================
export interface revenuetype {
    StationId: number;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
}
export interface revenuetypeday extends revenuetype {
    Date: string;
}
export interface revenuetypemonth extends revenuetype {
    Month: string;
}

export interface revenuetypeyear extends revenuetype {
    Year: string;
}

// ===============================
export interface Station{
    StationId: number,
    name: string,
}