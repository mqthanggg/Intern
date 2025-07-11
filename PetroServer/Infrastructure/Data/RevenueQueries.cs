
public static class RevenueQueries
{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SumRevenue = $@"     
        SELECT SUM(total_amount) AS TotalAmount, SUM(total_liters) AS TotalLiters FROM {Schema}.log
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