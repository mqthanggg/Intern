export interface LogRecord {
    stationId:number,
    name: number,
    fuelName: string,
    totalLiters: number,
    price: number
    totalAmount: number,
    time: Date,
    logTypeName: string;
}

export interface PagedResult<T> {
  data: T[];
  totalItems: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface Station{
    stationId: number,
    name: string,
}

