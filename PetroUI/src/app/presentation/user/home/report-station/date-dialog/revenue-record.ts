export interface revenuefuel {
    StationId: number;
    FuelName: string;
    Time: string;
    TotalAmount: number;
    TotalLiters: number;
}

export interface WSrevenuefuel {
    StationId: number;
    FuelName: string;
    Time: string;
    TotalAmount: number;
    TotalLiters: number;
}

//=======================================
export interface revenuetypeday{
    StationId: number;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
    Date: string;
}

export interface WSrevenuetypeday {
    StationId: number;
    LogTypeName: string;
    TotalAmount: number;
    TotalLiters: number;
    Date: string;
}
