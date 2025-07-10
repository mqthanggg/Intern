
public static class RevenueQueries
{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SumRevenue = $@"     
        SELECT 
            CASE
                WHEN CURRENT_TIME >= TIME '06:00:00' AND CURRENT_TIME < TIME '14:00:00' THEN 'Sáng'
                WHEN CURRENT_TIME >= TIME '14:00:00' AND CURRENT_TIME < TIME '22:00:00' THEN 'Chiều'
                ELSE 'Đêm'
            END AS ShiftNow,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        WHERE
            time::date = CURRENT_DATE
            AND (
                (CURRENT_TIME >= TIME '06:00:00' AND CURRENT_TIME < TIME '14:00:00' AND time::time BETWEEN TIME '06:00:00' AND TIME '14:00:00') OR
                (CURRENT_TIME >= TIME '14:00:00' AND CURRENT_TIME < TIME '22:00:00' AND time::time BETWEEN TIME '14:00:00' AND TIME '22:00:00') OR
                (CURRENT_TIME >= TIME '22:00:00' AND CURRENT_TIME < TIME '06:00:00' AND time::time BETWEEN TIME '22:00:00' AND TIME '06:00:00')
            )
    ";

    public static readonly string SumFuelbyName = $@"
        SELECT 
            fuel_name AS FuelName,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TongNhienLieu
        FROM 
            {Schema}.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            time::date = CURRENT_DATE
            AND (
                (CURRENT_TIME >= TIME '06:00:00' AND CURRENT_TIME < TIME '14:00:00' AND time::time BETWEEN TIME '06:00:00' AND TIME '14:00:00') OR
                (CURRENT_TIME >= TIME '14:00:00' AND CURRENT_TIME < TIME '22:00:00' AND time::time BETWEEN TIME '14:00:00' AND TIME '22:00:00') OR
                (CURRENT_TIME >= TIME '22:00:00' AND CURRENT_TIME < TIME '06:00:00' AND time::time BETWEEN TIME '22:00:00' AND TIME '06:00:00')
            )
            and station.station_id = @StationId
        GROUP BY fuel_name
        ORDER BY fuel_name
   ";

    public static readonly string SumFuelbyType = $@"
        SELECT 
            log_type AS LogType,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TongNhienLieu
        FROM 
            {Schema}.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            time::date = CURRENT_DATE
            AND (
                (CURRENT_TIME >= TIME '06:00:00' AND CURRENT_TIME < TIME '14:00:00' AND time::time BETWEEN TIME '06:00:00' AND TIME '14:00:00') OR
                (CURRENT_TIME >= TIME '14:00:00' AND CURRENT_TIME < TIME '22:00:00' AND time::time BETWEEN TIME '14:00:00' AND TIME '22:00:00') OR
                (CURRENT_TIME >= TIME '22:00:00' AND CURRENT_TIME < TIME '06:00:00' AND time::time BETWEEN TIME '22:00:00' AND TIME '06:00:00')
            )
            and station.station_id = @StationId
        GROUP BY log_type
        ORDER BY log_type
   ";
}