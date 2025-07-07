export interface DispenserRecord {
    dispenserId: number,
    name: number,
    price: number,
    longName: string,
    shortName: string,
    liter: number | undefined,
    totalAmount: number | undefined,
    status: string | undefined
}

export interface WSDispenserRecord{
    liter: number,
    price: number,
    state: string
}