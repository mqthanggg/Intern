export interface monthrevenuefuel {
    StationId: number;
    FuelName: string;
    Month: string;
    TotalAmount: number;
    TotalLiters: number;
}

export interface WSmonthrevenuefuel {
    StationId: number;
    FuelName: string;
    Month: string;
    TotalAmount: number;
    TotalLiters: number;
}

//=======================================
export interface revenuetypemonth{
    StationId: number;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
    Month: string;
}

export interface WSrevenuetypemonth {
    StationId: number;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
    Month: string;
}
