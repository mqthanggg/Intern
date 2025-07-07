export interface TankRecord {
    tankId: number,
    name: number,
    shortName: string,
    maxVolume: number,
    currentVolume: number | undefined,
    percentage: string | undefined
}

export interface WSTankRecord{
    current_volume: number
}
