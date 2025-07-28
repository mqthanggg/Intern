export interface yearrevenuefuel {
    StationId: number;
    FuelName: string;
    Year: string;
    TotalAmount: number;
    TotalLiters: number;
}

export interface WSyearrevenuefuel {
    StationId: number;
    FuelName: string;
    Year: string;
    TotalAmount: number;
    TotalLiters: number;
}

//=======================================
export interface revenuetypeyear{
    StationId: number;
    StationName: string;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
    Year: string;
}

export interface WSrevenuetypeyear {
    StationId: number;
    StationName: string;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
    Year: string;
}
