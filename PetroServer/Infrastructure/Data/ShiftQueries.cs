public static class ShiftQuery
{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SelectShift = $@"
        SELECT 
            shift_id,
            shift_type,
            start_time,
            end_time
        FROM {Schema}.shift
        ORDER BY
            shift_id
    ";
    public static readonly string SelectShiftById = $@"
        SELECT 
            shift_id,
            shift_type,
            start_time,
            end_time
        FROM {Schema}.shift
        WHERE
            shift_id = @ShiftId
    ";
    public static readonly string InsertShift = $@"
        INSERT INTO {Schema}.shift(
            shift_type,
            start_time,
            end_time,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @ShiftName,
            @StartTime,
            @EndTime,
            @CreatedBy,
            now(),
            @LastModifiedBy,
            now()
        )
    ";
    public static readonly string UpdateShift = $@"
        UPDATE {Schema}.shift
        SET
            shift_type =  @ShiftName,
            start_time = @StartTime,
            end_time = @EndTime,
            last_modified_by = @LastModifiedBy,
            last_modified_date = now()
        WHERE
            shift_id = @ShiftId
    ";
    public static readonly string DeleteShift = $@"
        DELETE FROM {Schema}.shift
        WHERE 
            shift_id = @ShiftId
    ";
}