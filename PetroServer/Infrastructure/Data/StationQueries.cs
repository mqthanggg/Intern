public static class StationQuery{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SelectStation = $@"
        SELECT 
            station_id,
            name,
            address,
        FROM {Schema}.station
    ";
    public static readonly string SelectStationById = $@"
        SELECT 
            station_id,
            name,
            address,
        FROM {Schema}.station
        WHERE
            station_id = @StationId
    ";
    public static readonly string InsertStation = $@"
        INSERT INTO {Schema}.station(
            name,
            address,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @Name,
            @Address,
            @CreatedBy,
            now(),
            @LastModifiedBy,
            now()
        )
    ";
    public static readonly string UpdateStation = $@"
        UPDATE {Schema}.station
        SET
            name = @Name,
            address = @Address,
            last_modified_by = @LastModifiedBy,
            last_modified_date = now()
        WHERE
            station_id = @StationId
    ";
    public static readonly string DeleteStation = $@"
        DELETE FROM {Schema}.station
        WHERE 
            station_id = @StationId
    ";
}