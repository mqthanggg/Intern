export interface UserRecord {
    userId: number,
    username: string,
    role: number,
    active: boolean
}

export interface UserRecordWithPage{
    users: UserRecord[],
    pageNumber: number
}

export interface UserFilter{
    username: string | null,
    role: number | null,
    active: number | null
}