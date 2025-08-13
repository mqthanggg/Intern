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
            LOCALTIMESTAMP(0),
            @LogType,
            @CreatedBy,
            LOCALTIMESTAMP(0),
            @LastModifiedBy,
            LOCALTIMESTAMP(0)
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
    // ========================= 1 CONDITION =========================
    public static readonly string SelectDispenserNameByStationId = $@"
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
        AND dp.name = @Name
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelNameByStationId = $@"
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
        AND log.fuel_name = @FuelName
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogTypeByStationId = $@"
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
        AND log.log_type = @LogType
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogByPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogByTotalAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogByTotalLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    // ========================= 2 CONDITION =========================
    public static readonly string SelectPeriodDispenerByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodFuelByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenerFuelByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND dp.name = @Name
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND dp.name = @Name
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.log_type = @LogType
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPriceByStationId = $@"
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
        AND dp.name = @Name
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserTotalLitersByStationId = $@"
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
        AND dp.name = @Name
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserTotalAmountByStationId = $@"
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
        AND dp.name = @Name
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelNamePriceByStationId = $@"
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
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelNameTotalLitersByStationId = $@"
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
        AND log.fuel_name = @FuelName
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelNameTotalAmountByStationId = $@"
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
        AND log.fuel_name = @FuelName
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogPriceByStationId = $@"
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
        AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.time DESC 
        LIMIT 30;    
    ";
    public static readonly string SelectLogTotalLitersByStationId = $@"
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
        AND log.log_type = @LogType
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.time DESC 
        LIMIT 30;    
    ";
    public static readonly string SelectLogTotalAmountByStationId = $@"
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
        AND log.log_type = @LogType
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.time DESC 
        LIMIT 30;    
    ";
    public static readonly string SelectPeriodPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodTotalLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodTotalAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogByPriceTotalLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND fuel.price BETWEEN @From AND @To
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogByPriceTotalAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogByTotalLiterAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.total_liters BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    // ========================= 3 CONDITION =========================
    public static readonly string SelectPeriodDispenerFuelByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND dp.name = @Name
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodDispenserLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND dp.name = @Name
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodFuelLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.log_type = @LogType
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodLogPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodLogLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodLogAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodPriceAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodPriceLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodAmountLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        AND log.total_amount BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodFuelPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodFuelAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPeriodFuelLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelPriceLiterByStationId = $@"
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
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelPriceAmountByStationId = $@"
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
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.time DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDipenserFuelLogByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.log_type = @LogType
        AND dp.name = @Name
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name = @FuelName
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name =@FuelName
        AND log.total_amount BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND dp.name = @Name
        AND log.total_amount BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND dp.name = @Name
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND dp.name = @Name
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPeriodPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND dp.name = @Name
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPeriodLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId
        AND dp.name = @Name 
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPeriodAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPriceAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPriceLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND fuel.price BETWEEN @From AND @To
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserAmountLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @From AND @To 
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogPriceAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogPriceLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogAmountLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.total_liters BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelLogPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name =@FuelName
        AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelLogLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name =@FuelName
        AND log.log_type = @LogType
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelLogAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name =@FuelName
        AND log.log_type = @LogType
        AND log.total_amount BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelAmountLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND  log.fuel_name = @FuelName
        AND log.total_liters BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectPriceAmountLitersByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromAmount AND @ToAmount
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    // ========================= 4 CONDITION =========================
    public static readonly string SelectDispenserLogFuelPeriodByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.log_type = @LogType
        AND dp.name = @Name  
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogFuelPriceByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogFuelAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND log.fuel_name = @FuelName
        AND log.total_amount BETWEEN @FromAmount AND @ToAmount
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogFuelLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND log.fuel_name = @FuelName
        AND log.total_liters BETWEEN @From AND @To
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogPricePeriodByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        AND log.""time"" BETWEEN @FromDate AND @
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogPriceAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogPriceLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogPeriodAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogPeriodLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserLogAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.log_type = @LogType
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelPricePeriodByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelPriceAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelPriceLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelPeriodAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name = @FuelName
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelPeriodLiteryStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name = @FuelName
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserFuelAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.fuel_name = @FuelName
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPricePeriodAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND fuel.price BETWEEN @From AND @To
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPricePeriodLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND fuel.price BETWEEN @From AND @To
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPriceAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectDispenserPeriodAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogFuelPricePeriodByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND dp.name = @Name
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogFuelPriceAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogFuelPriceLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogFuelPeriodAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.fuel_name = @FuelName
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogFuelPeriodLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.fuel_name = @FuelName
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogFuelAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        AND log.fuel_name = @FuelName
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogPricePeriodAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        AND log.fuel_name = @FuelName
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogPricePeriodLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogPriceAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
       AND log.log_type = @LogType
        AND fuel.price BETWEEN @From AND @To
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectLogPeriodAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.log_type = @LogType
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelPricePeriodAmountByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelPricePeriodLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelPriceAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND fuel.price BETWEEN @From AND @To
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
    public static readonly string SelectFuelPeriodAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND log.fuel_name = @FuelName
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
     public static readonly string SelectPricePeriodAmountLiterByStationId = $@"
        SELECT 
            dp.name AS Name, 
            log.fuel_name AS FuelName, 
            log.total_liters AS TotalLiters, 
            fuel.price AS Price, 
            log.total_amount AS TotalAmount, 
            log.""time"" AS Time,
            log.log_type AS LogType 
        FROM {Schema}.""log"" as log
        INNER JOIN {Schema}.dispenser as dp  ON dp.dispenser_id = log.dispenser_id 
        INNER JOIN {Schema}.fuel as fuel ON dp.fuel_id = fuel.fuel_id
        WHERE dp.station_id = @StationId 
        AND fuel.price BETWEEN @From AND @To
        AND log.""time"" BETWEEN @FromDate AND @ToDate
        AND log.total_amount BETWEEN @FromTotal AND @ToTotal
        AND log.total_liters BETWEEN @FromTotal AND @ToTotal
        ORDER BY log.""time"" DESC 
        OFFSET @Offset 
        LIMIT @PageSize
    ";
}