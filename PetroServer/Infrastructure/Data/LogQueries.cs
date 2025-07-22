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
    public static readonly string DeleteLog = $@"
        DELETE FROM {Schema}.log
        WHERE 
            log_id = @LogId
    ";

    public static readonly string SelectLogByStationId = $@"
        SELECT 
            dp.name, 
            log.fuel_name, 
            log.total_liters, 
            fuel.price, 
            log.total_amount, 
            log.time,
            log.log_type 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp 
        ON 
            dp.dispenser_id = log.dispenser_id 
        AND 
            dp.station_id = @StationId
        INNER JOIN {Schema}.fuel as fuel 
        ON 
            dp.fuel_id = fuel.fuel_id
        ORDER BY
            log.log_id
    ";
    public static readonly string UpdateLogTime = $@"
        UPDATE {Schema}.log
        SET 
            time = date_trunc('day', CURRENT_DATE) + (time::time),
            last_modified_date = now()
    ";

}