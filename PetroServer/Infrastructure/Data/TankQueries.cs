public static class TankQuery{
    private static readonly string Schema  = Env.GetString("SCHEMA");
    public static readonly string SelectTank = $@"
        SELECT
            name,
            max_volume
        FROM {Schema}.tank
        ORDER BY
            tank_id
    ";
    public static readonly string SelectTankById = $@"
        SELECT
            name,
            max_volume
        FROM {Schema}.tank
        WHERE
            tank_id = @TankId
    ";
    public static readonly string InsertTank = $@"
        INSERT INTO {Schema}.tank(
            fuel_id,
            station_id,
            name,
            max_volume,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @FuelId,
            @StationId,
            @Name,
            @MaxVolume,
            @CreatedBy,
            @CreatedDate,
            @LastModifiedBy,
            @LastModifiedDate
        )
    ";
    public static readonly string UpdateTank = $@"
        UPDATE {Schema}.tank
        SET
            name = @Name,
            max_volume = @MaxVolume,
            last_modified_by = @LastModifiedBy,
            last_modified_date = @LastModifiedDate
        WHERE
            tank_id = @TankId
    ";
    public static readonly string DeleteTank = $@"
        DELETE FROM {Schema}.tank
        WHERE
            tank_id = @TankId
    ";
    public static readonly string SelectTankByStationId = $@"
        SELECT 
            t.name, 
            f.short_name, 
            t.max_volume 
        FROM {Schema}.tank as t
        INNER JOIN {Schema}.fuel as f 
        ON 
            f.fuel_id = t.fuel_id 
        AND 
            t.station_id = @StationId
        ORDER BY
            tank_id
    ";
}