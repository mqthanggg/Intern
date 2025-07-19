export interface revenuestationyear{
    StationId :number;
    StationName: String;
    Year: string;
    TotalRevenue: number,
    TotalProfit:number;
}

export interface WSrevenuestationyear{
    id: number;
    name: string;
    year: string;
    revenue:number;
    profit:number;
}

// ===============================
export interface revenuefuelyear{
    StationId :number;
    Year: string;
    FuelName: string;
    TotalAmount: number;
    TotalLiters:number;
}

export interface WSrevenuefuelyear{
    id: number;
    year:string;
    fuelname: string;
    amount:number;
    liters:number;
}

// ===============================
export interface revenuetypeyear{
    StationId :number;
    Year: string;
    LogTypeName: string;
    TotalAmount: number;
    totalLiters:number;
}

export interface WSrevenuetypeyear{
    id: number;
    year:string;
    name: string;
    amount:number;
    liters:number;
}

