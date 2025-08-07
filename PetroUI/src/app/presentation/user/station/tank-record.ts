export interface TankRecord {
    stationId: number,
    tankId: number,
    name: number,
    shortName: string,
    maxVolume: number,
    currentVolume: number | undefined,
    percentage: string | undefined
}

export interface WSTankRecord{
    stationId: number,
    tankId: number,
    name: number,
    shortName: string,
    maxVolume: number,
    currentVolume: number | undefined,
    percentage: string | undefined
}
