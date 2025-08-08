export interface DispenserRecord {
    // stationId:number,
    dispenserId: number,
    name: number,  // Number
    price: number,  
    longName: string,
    shortName: string,
    liter: number | undefined,
    totalAmount: number | undefined,
    status: string | undefined,
}

export interface WSDispenserRecord{
    price: number,  
    liter: number | undefined,
    state: number | undefined,
}