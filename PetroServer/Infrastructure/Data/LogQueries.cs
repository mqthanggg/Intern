public static class LogQuery
{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SelectLog = $@"
        SELECT 
            log_id,
            fuel_name,
            total_liters,
            total_amount,
            time,
            log_type
        FROM {Schema}.log
    ";
    public static readonly string SelectLogById = $@"
        SELECT 
            log_id,
            fuel_name,
            total_liters,
            total_amount,
            time,
            log_type
        FROM {Schema}.log
        WHERE
            log_id = @LogId
    ";
    public static readonly string InsertLog = $@"
        INSERT INTO {Schema}.log(
            dispenser_id,
            fuel_name,
            total_liters,
            total_amount,
            time,
            log_type,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @DispenserId,
            @FuelName,
            @TotalLiters,
            @TotalAmount,
            now(),
            @LogType,
            @CreatedBy,
            now(),
            @LastModifiedBy,
            now()
        )
    ";
    public static readonly string UpdateLog = $@"
        UPDATE {Schema}.log
        SET
            fuel_name = @FuelName,
            total_liters = @TotalLiters,
            total_amount = @TotalAmount,
            time = @Time,
            log_type = @LogType,            
            last_modified_by = @LastModifiedBy,
            last_modified_date = now()
        WHERE
            log_id = @LogId
    ";
    public static readonly string UpdateLogTime = $@"
        UPDATE {Schema}.log
        SET 
            time = date_trunc('day', CURRENT_DATE) + (time::time),
            last_modified_date = now()
    ";
    public static readonly string DeleteLog = $@"
        DELETE FROM {Schema}.log
        WHERE 
            log_id = @LogId
    ";
    public static readonly string SelectLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.time AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE DATE(log.time) = CURRENT_DATE AND dp.station_id = @StationId
        ORDER BY log.time DESC 
        LIMIT 30;    
    ";
    public static readonly string SelectFullLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.time AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId
        ORDER BY log.time DESC 
    ";
    public static readonly string SelectPageLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.time AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string CountLogByStationId = $@"
        SELECT 
          COUNT(*)
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId
    ";
    //==========================================
    public static readonly string SelectDispenserNameByStationId = $@"
        SELECT DISTINCT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.time AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId
        AND dp.name = @Name
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelNameByStationId = $@"
        SELECT DISTINCT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.time AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId
        AND log.fuel_name = @FuelName
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogTypeByStationId = $@"
        SELECT DISTINCT 
           dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.time AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId
        AND log.log_type = @LogType
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDateByStationId = $@"
        SELECT DISTINCT 
           dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.time AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.""time""::date = @Time
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodByStationId = $@"
        SELECT DISTINCT 
           dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.time AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.""time"" BETWEEN @ToDate AND @FromDate
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
}