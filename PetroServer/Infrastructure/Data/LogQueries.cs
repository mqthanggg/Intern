public static class LogQuery{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SelectLog = $@"
        SELECT 
            log_id,
            fuel_name,
            total_liters,
            total_amount,
            time
        FROM {Schema}.log
    ";
    public static readonly string SelectLogById = $@"
        SELECT 
            log_id,
            fuel_name,
            total_liters,
            total_amount,
            time
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
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @DispenserId,
            @FuelName,
            @TotalLiters,
            @TotalAmount,
            @Time,
            @CreatedBy,
            @CreatedDate,
            @LastModifiedBy,
            @LastModifiedDate
        )
    ";
    public static readonly string UpdateLog = $@"
        UPDATE {Schema}.log
        SET
            fuel_name = @FuelName,
            total_liters = @TotalLiters,
            total_amount = @TotalAmount,
            time = @Time,
            last_modified_by = @LastModifiedBy,
            last_modified_date = @LastModifiedDate
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
            log.time 
        FROM {Schema}.log as log
        INNER JOIN {Schema}.dispenser as dp 
        ON 
            dp.dispenser_id = log.dispenser_id 
        AND 
            dp.station_id = @StationId
        INNER JOIN {Schema}.fuel as fuel 
        ON 
            dp.fuel_id = fuel.fuel_id
    ";
}