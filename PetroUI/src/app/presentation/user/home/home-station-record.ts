export interface totalrevenue {
    TotalRevenue: number;
    TotalProfit: number;
    TotalLiters: number;
}

//==================================
export interface totalFuelName {
    FuelName: string;
    TotalAmount: number,
    TotalLiters: number
}
export interface WStotalFuelName {
    FuelName: string;
    TotalAmount: number,
    TotalLiters: number
}

//==================================
export interface totalrevenue7day {
   Date: string,
    StationId: number,
    StationName: string,
    TotalRevenue: number,
    TotalLiters: number
}

//==================================
export interface totalLogType {
    LogTypeName: string;
    TotalAmount: number,
}
export interface WStotalLogType {
    LogTypeName: string;
    TotalAmount: number,
}

//==================================
export interface totalStationName extends totalrevenue {
    StationId: number;
    StationName: string;
}
