export interface revenuestationmonth{
    stationId :number;
    stationName: String;
    Month: string;  //
    TotalRevenue: number,
    TotalProfit:number;
}

export interface WSrevenuestationmonth{
    id: number;
    name: string;
    Month:string;
    revenue:number;
    profit:number;
}

// ===============================
export interface revenuefuelmonth{
    StationId :number;
    Month: string;
    FuelName: string;
    TotalAmount: number;
    TotalLiters:number;
}

export interface WSrevenuefuelmonth{
    id: number;
    month:string;
    fuelname: string;
    amount:number;
    liters:number;
}

// ===============================
export interface revenuetypemonth{
    StationId :number;
    Month: string;
    logTypeName: string;
    totalAmount: number;
    totalLiters:number;
}

export interface WSrevenuetypemonth{
    id: number;
    month:string;
    name: string;
    amount:number;
    liters:number;
}
