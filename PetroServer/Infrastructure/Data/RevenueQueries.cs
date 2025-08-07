
public static class RevenueQueries
{
    private static readonly string Schema = Env.GetString("SCHEMA");

    // Report sum revenue of the stations in the system
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
    public static readonly string SumRevenueFullByName = $@"
        SELECT 
            fuel_name AS FuelName, 
            SUM(total_liters) AS TotalLiters,
            SUM(total_amount) AS TotalAmount
        FROM petro_application.log
        GROUP BY fuel_name
        ORDER BY fuel_name
    ";
    public static readonly string SumRevenueFullByType = $@"
        SELECT 
            log_type AS LogType, 
            SUM(total_liters) AS TotalLiters,
            SUM(total_amount) AS TotalAmount
        FROM petro_application.log
        GROUP BY log_type
        ORDER BY log_type
    ";
    public static readonly string SumRevenueBytype7day = $@"
        SELECT 
            log_type AS LogType,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM petro_application.log
        WHERE ""time""::date >= CURRENT_DATE - INTERVAL '7 days'
        GROUP BY log_type
        ORDER BY log_type ASC
    ";
    public static readonly string SumRevenueByDay = $@"
        SELECT 
            DATE(l.""time"") AS Date,
            s.name AS StationName,
            SUM(l.total_amount) AS TotalAmount,
            SUM(l.total_liters) AS TotalLiters
        FROM petro_application.log l
        JOIN petro_application.dispenser d ON d.dispenser_id = l.dispenser_id
        JOIN petro_application.station s ON s.station_id = d.station_id
        WHERE l.""time""::date >= CURRENT_DATE - INTERVAL '6 days'
        GROUP BY DATE(l.""time""), s.name
        ORDER BY DATE(l.""time""), s.name;
    ";

    // Report sum revenue of each stations in the system
    public static readonly string SumRevenueStation = $@"     
        WITH total_sales AS (
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
            s.station_id AS StationId,
            s.name AS StationName,
            COALESCE(ts.total_revenue, 0) AS TotalRevenue,
            COALESCE(tp.total_import, 0) AS TotalImport,
            COALESCE(ts.total_liters, 0) AS TotalLiters,
            (COALESCE(ts.total_revenue, 0) - COALESCE(tp.total_import, 0)) AS TotalProfit
        FROM petro_application.station s
        JOIN total_sales ts ON s.station_id = ts.station_id
        JOIN total_purchases tp ON s.station_id = tp.station_id
        WHERE s.station_id = @StationId
        ORDER BY s.station_id ASC;
    ";
    public static readonly string SumRevenueStationDay = $@"
        WITH all_dates AS (
            SELECT DATE(l.""time"") AS ngay
            FROM petro_application.dispenser d
            JOIN petro_application.log l ON l.dispenser_id = d.dispenser_id
            WHERE d.station_id = @StationId
            UNION
            SELECT DATE(r.receipt_date) AS ngay
            FROM petro_application.receipt r
            WHERE r.station_id = @StationId
        ),
        total_sales AS (
            SELECT 
                DATE(l.""time"") AS ngay,
                SUM(l.total_amount) AS total_revenue,
                SUM(l.total_liters) AS total_liters
            FROM petro_application.dispenser d
            JOIN petro_application.log l ON l.dispenser_id = d.dispenser_id
            WHERE d.station_id = @StationId
            GROUP BY DATE(l.""time"")
        ),
        total_purchases AS (
            SELECT 
                DATE(r.receipt_date) AS ngay,
                SUM(r.total_import) AS total_import
            FROM petro_application.receipt r
            WHERE r.station_id = @StationId
            GROUP BY DATE(r.receipt_date)
        )
        SELECT 
            s.station_id AS StationId,
            s.name AS StationName,
            d.ngay Date,
            COALESCE(ts.total_revenue, 0) AS TotalRevenue,
            (COALESCE(ts.total_revenue, 0) - COALESCE(tp.total_import, 0)) AS TotalProfit
        FROM all_dates d
        LEFT JOIN total_sales ts ON d.ngay = ts.ngay
        LEFT JOIN total_purchases tp ON d.ngay = tp.ngay
        JOIN petro_application.station s ON s.station_id = @StationId
        ORDER BY d.ngay;
    ";
    public static readonly string SumRevenueStationMonth = $@"
        WITH all_dates AS (
            SELECT TO_CHAR(l.""time"", 'MM-YYYY') AS thang
            FROM petro_application.dispenser d
            JOIN petro_application.log l ON l.dispenser_id = d.dispenser_id
            WHERE d.station_id = @StationId
            UNION
            SELECT TO_CHAR(r.receipt_date, 'MM-YYYY') AS thang
            FROM petro_application.receipt r
            WHERE r.station_id = @StationId
        ),
        total_sales AS (
            SELECT 
                TO_CHAR(l.""time"", 'MM-YYYY') AS thang,
                SUM(l.total_amount) AS total_revenue,
                SUM(l.total_liters) AS total_liters
            FROM petro_application.dispenser d
            JOIN petro_application.log l ON l.dispenser_id = d.dispenser_id
            WHERE d.station_id = @StationId
            GROUP BY  TO_CHAR(l.""time"", 'MM-YYYY')
        ),
        total_purchases AS (
            SELECT 
                TO_CHAR(r.receipt_date, 'MM-YYYY') AS thang,
                SUM(r.total_import) AS total_import
            FROM petro_application.receipt r
            WHERE r.station_id = @StationId
            GROUP BY TO_CHAR(r.receipt_date, 'MM-YYYY')
        )
        SELECT 
            s.station_id AS StationId,
            s.name AS StationName,
            d.thang AS Month,
            COALESCE(ts.total_revenue, 0) AS TotalRevenue,
            (COALESCE(ts.total_revenue, 0) - COALESCE(tp.total_import, 0)) AS TotalProfit
        FROM all_dates d
        LEFT JOIN total_sales ts ON d.thang = ts.thang
        LEFT JOIN total_purchases tp ON d.thang = tp.thang
        JOIN petro_application.station s ON s.station_id = @StationId
        ORDER BY d.thang;
    ";
    public static readonly string SumRevenueStationYear = $@"
        WITH all_years AS (
            SELECT TO_CHAR(l.""time"", 'YYYY') AS nam
            FROM petro_application.dispenser d
            JOIN petro_application.log l ON l.dispenser_id = d.dispenser_id
            WHERE d.station_id = @StationId
            UNION
            SELECT TO_CHAR(r.receipt_date, 'YYYY') AS nam
            FROM petro_application.receipt r
            WHERE r.station_id = 1
        ),
        total_sales AS (
            SELECT 
                TO_CHAR(l.""time"", 'YYYY') AS nam,
                SUM(l.total_amount) AS total_revenue,
                SUM(l.total_liters) AS total_liters
            FROM petro_application.dispenser d
            JOIN petro_application.log l ON l.dispenser_id = d.dispenser_id
            WHERE d.station_id = 1
            GROUP BY  TO_CHAR(l.""time"", 'YYYY')
        ),
        total_purchases AS (
            SELECT 
                TO_CHAR(r.receipt_date, 'YYYY') AS nam,
                SUM(r.total_import) AS total_import
            FROM petro_application.receipt r
            WHERE r.station_id = 1
            GROUP BY TO_CHAR(r.receipt_date, 'YYYY')
        )
        SELECT 
            s.station_id AS StationId,
            s.name AS StationName,
            y.nam AS Year,
            COALESCE(ts.total_revenue, 0) AS TotalRevenue,
            (COALESCE(ts.total_revenue, 0) - COALESCE(tp.total_import, 0)) AS TotalProfit
        FROM all_years y
        LEFT JOIN total_sales ts ON y.nam = ts.nam
        LEFT JOIN total_purchases tp ON y.nam = tp.nam
        JOIN petro_application.station s ON s.station_id = @StationId
        ORDER By y.nam;
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
    public static readonly string SumFuelbyNameGetDay = @"
        SELECT 
            fuel_name AS FuelName,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            petro_application.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            log.""time""::DATE = @Time
            AND station.station_id = @StationId
        GROUP BY fuel_name
        ORDER BY fuel_name;
    ";
    public static readonly string SumFuelbyTypeGetDay = @"
        SELECT 
            log_type AS LogType, 
            SUM(total_amount) AS TotalAmount, 
            SUM(total_liters) AS TotalLiters
        FROM 
            petro_application.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE  
            log.""time""::date = @Time
            AND station.station_id = @StationId
        GROUP BY log_type
        ORDER BY log_type
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
    public static readonly string SumFuelbyNameGetMonth = $@"
        SELECT 
            fuel_name AS FuelName,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            petro_application.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            EXTRACT(MONTH FROM log.""time"") = @Month
            AND EXTRACT(YEAR FROM log.""time"") = @Year
            AND station.station_id = 1
        GROUP BY fuel_name
        ORDER BY fuel_name;
    ";
    public static readonly string SumFuelbyTypeGetMonth = $@"
        SELECT 
            log_type AS LogType, 
            SUM(total_amount) AS TotalAmount, 
            SUM(total_liters) AS TotalLiters
        FROM 
            petro_application.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            EXTRACT(MONTH FROM log.""time"") = @Month
            AND EXTRACT(YEAR FROM log.""time"") = @Year
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
    public static readonly string SumFuelbyNameGetYear = @"
        SELECT 
            fuel_name AS FuelName,
            SUM(total_amount) AS TotalAmount,
            SUM(total_liters) AS TotalLiters
        FROM 
            petro_application.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE
            EXTRACT(YEAR FROM log.""time"") = @Year
            AND station.station_id = @StationId
        GROUP BY fuel_name
        ORDER BY fuel_name;
    ";
    public static readonly string SumFuelbyTypeGetYear = @"
        SELECT 
            log_type AS LogType, 
            SUM(total_amount) AS TotalAmount, 
            SUM(total_liters) AS TotalLiters
        FROM 
            petro_application.log
        JOIN 
            petro_application.dispenser ON log.dispenser_id = dispenser.dispenser_id
        JOIN 
            petro_application.station ON station.station_id = dispenser.station_id
        WHERE 
            EXTRACT(YEAR FROM log.""time"") = @Year
            AND station.station_id = @StationId
        GROUP BY log_type
        ORDER BY log_type
    ";
}