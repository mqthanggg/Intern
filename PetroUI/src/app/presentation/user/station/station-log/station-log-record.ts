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

export interface StationRecord{
  stationId: number,
  name: string,
}

export interface FuelRecord{
  shortName: string,
}

// =========================================================
export interface GetPeriodResponse {
  stationId: number,
  fromDate: Date,
  toDate: Date,
  page: number,
  pageSize: number
}
export interface DispenserFuelRecord extends GetPeriodResponse{
  name: string,
  fuelName: string,
}

export interface DispenserLogTypeRecord {
  name: number,
  logType: number,
}

export interface FullConditionRecord extends GetPeriodResponse {
  name: number,
  fuelName: string,
  logType: number,
}


