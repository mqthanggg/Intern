export interface LogRecord {
    StationId:number,
    Name: number,
    FuelName: string,
    TotalLiters: number,
    Price: number
    TotalAmount: number,
    Time: Date,
    LogTypeName: string;
}

export interface Station{
    StationId: number,
    name: string,
}