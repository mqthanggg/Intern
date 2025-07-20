export interface revenuestationday{
    StationId :number;
    StationName: String;
    Date: string;
    TotalRevenue: number,
    TotalProfit:number;
}

export interface WSrevenuestationday{
    id: number;
    name: string;
    date:string;
    revenue:number;
    profit:number;
}

// ===============================
export interface revenuefuelday{
    StationId :number;
    Date: string;
    FuelName: string;
    TotalAmount: number;
    TotalLiters:number;
}

export interface WSrevenuefuelday{
    id: number;
    date:string;
    fuelname: string;
    amount:number;
    liters:number;
}

// ===============================
export interface revenuetypeday{
    StationId :number;
    Date: string;
    LogTypeName: string;
    TotalAmount: number;
    totalLiters:number;
}

export interface WSrevenuetypeday{
    id: number;
    date:string;
    logname: string;
    amount:number;
    liters:number;
}

