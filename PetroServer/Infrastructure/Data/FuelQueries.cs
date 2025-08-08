public static class FuelQuery
{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string InsertFuel = $@"
        INSERT INTO {Schema}.fuel(
            short_name,
            long_name,
            price,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        )
        VALUES (
            @ShortName,
            @LongName,
            @Price,
            @CreatedBy,
            now(),
            @LastModifiedBy,
            now()
        )
    ";
    public static readonly string UpdateFuel = $@"
        UPDATE {Schema}.fuel
        SET
            short_name = @ShortName,
            long_name = @LongName,
            price = @Price,
            last_modified_by = @LastModifiedBy,
            last_modified_date = now()
        WHERE
            fuel_id = @FuelId
    ";
    public static readonly string DeleteFuel = $@"
        DELETE FROM {Schema}.fuel
        WHERE 
            fuel_id = @FuelId
    ";
    public static readonly string SelectFuelShortNameById = $@"
        SELECT 
            short_name
        FROM {Schema}.fuel
        WHERE
            fuel_id = @FuelId
    ";
    public static readonly string SelectFuel = $@"
        SELECT * FROM {Schema}.fuel
    ";
}