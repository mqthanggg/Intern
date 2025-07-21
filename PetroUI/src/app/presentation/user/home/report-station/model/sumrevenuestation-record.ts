export interface revenuestation {
    stationId: number;
    stationName: string;
    totalRevenue: number,
    totalLiters: number,
    totalProfit: number;
}

export interface WSrevenuestation {
    id: number;
    name: string;
    revenue: number;
    profit: number;
    liter: number;
}

export interface revenuestationday extends revenuestation {
    date: string;
}

export interface WSrevenuestationday extends WSrevenuestation {
    date: string;
}

export interface revenuestationmonth extends revenuestation {
    month: string;
}

export interface WSrevenuestationmonth extends WSrevenuestation {
    month: string;
}

export interface revenuestationyear extends revenuestation {
    year: string;
}

export interface WSrevenuestationyear extends WSrevenuestation {
    year: string;
}
// ===============================
export interface revenuefuel {
    stationId: number;
    fuelName: string;
    totalAmount: number;
    totalLiters: number;
}

export interface WSrevenuefuel {
    id: number;
    fuelName: string;
    amount: number;
    liters: number;
}

export interface revenuefuelday extends revenuefuel {
    date: string;
}

export interface WSrevenuefuelday extends WSrevenuefuel {
    date: string;
}

export interface revenuefuelmonth extends revenuefuel {
    month: string;
}

export interface WSrevenuefuelmonth extends WSrevenuefuel {
    month: string;
}

export interface revenuefuelyear extends revenuefuel {
    year: string;
}

export interface WSrevenuefuelyear extends WSrevenuefuel {
    year: string;
}
// ===============================
export interface revenuetype {
    stationId: number;
    logTypeName: string;
    totalAmount: number;
    totalLiters: number;
}

export interface WSrevenuetype {
    id: number;
    logName: string;
    amount: number;
    liters: number;
}

export interface revenuetypeday extends revenuetype {
    date: string;
}

export interface WSrevenuetypeday extends WSrevenuetype {
    date: string;
}

export interface revenuetypemonth extends revenuetype {
    month: string;
}

export interface WSrevenuetypemonth extends WSrevenuetype {
    month: string;
}

export interface revenuetypeyear extends revenuetype {
    year: string;
}

export interface WSrevenuetypeyear extends WSrevenuetype {
    year: string;
}