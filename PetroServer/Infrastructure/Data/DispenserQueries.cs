public static class DispenserQuery{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string GetDispenserByStationId = $@"
        SELECT 
            dp.name, 
            f.price, 
            f.long_name, 
            f.short_name 
        FROM {Schema}.dispenser as dp
        INNER JOIN {Schema}.fuel as f 
        ON 
            f.fuel_id = dp.fuel_id 
        AND 
            dp.station_id = @StationId
    ";
    public static readonly string SelectDispenser = $@"
        SELECT 
            name
        FROM {Schema}.dispenser
    ";
    public static readonly string SelectDispenserById = $@"
        SELECT 
            name
        FROM {Schema}.dispenser
        WHERE
            dispenser_id = @DispenserId
    ";
    public static readonly string InsertDispenser = $@"
        INSERT INTO {Schema}.dispenser (
            station_id,
            tank_id,
            fuel_id,
            name,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @StationId,
            @TankId,
            @FuelId,
            @Name,
            @CreatedBy,
            @CreatedDate,
            @LastModifiedBy,
            @LastModifiedDate
        )
    ";
    public static readonly string UpdateDispenser = $@"
        UPDATE {Schema}.dispenser
        SET
            name = @Name
        WHERE
            dispenser_id = @DispenserId
    ";
    public static readonly string DeleteDispenser = $@"
        DELETE FROM {Schema}.dispenser
        WHERE
            dispenser_id = @DispenserId
    ";
}