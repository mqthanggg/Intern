
public static class RevenueQueries
{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SumRevenue = $@"     
        WITH 
            total_sales AS (
                SELECT SUM(l.total_amount) AS total_revenue, SUM(total_liters) AS total_liters
                FROM {Schema}.log l
            ),
            total_purchases AS (
                SELECT SUM(r.total_import) AS total_import
                FROM {Schema}.receipt r
            )
        SELECT 
            COALESCE(ts.total_liters, 0) AS totalLiters,
            COALESCE(ts.total_revenue, 0) AS TotalRevenue,
            COALESCE(tp.total_import, 0) AS TotalImport,
            (COALESCE(ts.total_revenue, 0) - COALESCE(tp.total_import, 0)) AS TotalProfit
        FROM total_sales ts, total_purchases tp;
    ";
    public static readonly string SumRevenueByStation = $@"
       WITH 
            total_sales AS (
                SELECT 
                    s.station_id, 
                    SUM(l.total_amount) AS total_revenue,
                    SUM(l.total_liters) AS total_liters
                FROM petro_application.station s
                JOIN petro_application.dispenser d ON d.station_id = s.station_id
                JOIN petro_application.log l ON l.dispenser_id = d.dispenser_id
                GROUP BY s.station_id
            ),
            total_purchases AS (
                SELECT 
                    station_id, 
                    SUM(total_import) AS total_import
                FROM petro_application.receipt
                GROUP BY station_id
            )
        SELECT 
            s.station_id,
            s.name AS StationName,
            COALESCE(ts.total_revenue, 0) AS TotalRevenue,
            COALESCE(ts.total_liters, 0) AS totalLiters,
            COALESCE(tp.total_import, 0) AS TotalImport,
            (COALESCE(ts.total_revenue, 0) - COALESCE(tp.total_import, 0)) AS TotalProfit
        FROM petro_application.station s
        LEFT JOIN total_sales ts ON s.station_id = ts.station_id
        LEFT JOIN total_purchases tp ON s.station_id = tp.station_id
        ORDER BY s.station_id ASC;
    ";
    public static readonly string SumRevenueBytype = $@"
        SELECT 
            log_type AS LogType,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM petro_application.log
        GROUP BY log_type
        ORDER BY log_type ASC
    ";
  // SumRevenue query returns the total revenue AND liters for the current shift (morning, afternoon, or night).
    public static readonly string SumFuelbyName = $@"
        SELECT 
            fuel_name AS FuelName,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        JOIN 
            {Schema}.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            {Schema}.station ON station.station_id = dispenser.station_id
        WHERE
            time::date = CURRENT_DATE
            AND (
                (CURRENT_TIME >= TIME '06:00:00' AND CURRENT_TIME < TIME '14:00:00' AND time::time BETWEEN TIME '06:00:00' AND TIME '14:00:00') OR
                (CURRENT_TIME >= TIME '14:00:00' AND CURRENT_TIME < TIME '22:00:00' AND time::time BETWEEN TIME '14:00:00' AND TIME '22:00:00') OR
                (CURRENT_TIME >= TIME '22:00:00' AND CURRENT_TIME < TIME '06:00:00' AND time::time BETWEEN TIME '22:00:00' AND TIME '06:00:00')
            )
            AND station.station_id = @StationId
        GROUP BY fuel_name
        ORDER BY fuel_name
   ";
    public static readonly string SumFuelbyType = $@"
        SELECT 
            log_type AS LogType,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        JOIN 
            {Schema}.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            {Schema}.station ON station.station_id = dispenser.station_id
        WHERE
            time::date = CURRENT_DATE
            AND (
                (CURRENT_TIME >= TIME '06:00:00' AND CURRENT_TIME < TIME '14:00:00' AND time::time BETWEEN TIME '06:00:00' AND TIME '14:00:00') OR
                (CURRENT_TIME >= TIME '14:00:00' AND CURRENT_TIME < TIME '22:00:00' AND time::time BETWEEN TIME '14:00:00' AND TIME '22:00:00') OR
                (CURRENT_TIME >= TIME '22:00:00' AND CURRENT_TIME < TIME '06:00:00' AND time::time BETWEEN TIME '22:00:00' AND TIME '06:00:00')
            )
            AND station.station_id = @StationId
        GROUP BY log_type
        ORDER BY log_type
   ";

    // SumRevenue query returns the total revenue AND liters for the current Date
    public static readonly string SumFuelbyNameDay = $@"
        SELECT 
            fuel_name AS FuelName,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            time::date = CURRENT_DATE
            AND station.station_id = @StationId
        GROUP BY fuel_name
        ORDER BY fuel_name
   ";
    public static readonly string SumFuelbyTypeDay = $@"
        SELECT 
            log_type AS LogType,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            time::date = CURRENT_DATE
            AND station.station_id = @StationId
        GROUP BY log_type
   ";

    // SumRevenue query returns the total revenue AND liters for the current month
    public static readonly string SumFuelbyNameMonth = $@"
        SELECT 
            fuel_name AS FuelName,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            EXTRACT(MONTH FROM log.""time"") = EXTRACT(MONTH FROM CURRENT_DATE)
            AND station.station_id = @StationId
        GROUP BY fuel_name
        ORDER BY fuel_name
   ";
    public static readonly string SumFuelbyTypeMonth = $@"
        SELECT 
            log_type AS LogType,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            EXTRACT(MONTH FROM log.""time"") = EXTRACT(MONTH FROM CURRENT_DATE)
            AND station.station_id = @StationId
        GROUP BY log_type
        ORDER BY log_type
   ";

   // SumRevenue query returns the total revenue AND liters for the current year
    public static readonly string SumFuelbyNameYear = $@"
        SELECT 
            fuel_name AS FuelName,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            EXTRACT(YEAR FROM log.""time"") = EXTRACT(YEAR FROM CURRENT_DATE)
            AND station.station_id = @StationId
        GROUP BY fuel_name
        ORDER BY fuel_name
   ";
    public static readonly string SumFuelbyTypeYear = $@"
        SELECT 
            log_type AS LogType,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            {Schema}.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            EXTRACT(YEAR FROM log.""time"") = EXTRACT(YEAR FROM CURRENT_DATE)
            AND station.station_id = @StationId
        GROUP BY log_type
        ORDER BY log_type
   ";
}