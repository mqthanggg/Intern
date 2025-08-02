public static class DispenserQuery{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string GetDispenserByStationId = $@"
        SELECT 
            dp.station_id AS StationId,
            dp.dispenser_id AS DispenserId,
            dp.name AS Name, 
            f.price AS Price, 
            f.long_name AS LongName, 
            f.short_name AS ShortName
        FROM {Schema}.dispenser as dp
        INNER JOIN {Schema}.fuel as f 
        ON 
            f.fuel_id = dp.fuel_id 
        AND 
            dp.station_id = @StationId
        ORDER BY
            dp.name
    ";
    public static readonly string SelectDispenser = $@"
        SELECT 
            dispenser_id,
            name
        FROM {Schema}.dispenser
        ORDER BY
            dp.dispenser_id
    ";
    public static readonly string SelectDispenserFuelId = $@"
        SELECT 
            dispenser_id,
            fuel_id
        FROM {Schema}.dispenser
        WHERE
            dispenser_id = @DispenserId
    ";
    public static readonly string SelectDispenserById = $@"
        SELECT 
            dispenser_id,
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
            now(),
            @LastModifiedBy,
            now()
        )
    ";
    public static readonly string UpdateDispenser = $@"
        UPDATE {Schema}.dispenser
        SET
            name = @Name,
            last_modified_by = @LastModifiedBy,
            last_modified_date = now()
        WHERE
            dispenser_id = @DispenserId
    ";
    public static readonly string DeleteDispenser = $@"
        DELETE FROM {Schema}.dispenser
        WHERE
            dispenser_id = @DispenserId
    ";
}