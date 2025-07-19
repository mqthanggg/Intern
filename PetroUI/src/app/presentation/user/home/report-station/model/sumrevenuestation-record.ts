export interface revenuestation{
    StationId :number;
    stationName: string;
    TotalRevenue: number,
    TotalLiters: number,
    TotalProfit:number;
}

export interface WSrevenuestation{
    id: number;
    revenue:number;
    profit:number;
    liter:number;
}
