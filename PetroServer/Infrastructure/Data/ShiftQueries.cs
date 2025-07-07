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
            end_time
        ) VALUES (
            @ShiftName,
            @StartTime,
            @EndTime
        )
    ";
    public static readonly string UpdateShift = $@"
        UPDATE {Schema}.shift
        SET
            shift_type =  @ShiftName,
            start_time = @StartTime,
            end_time = @EndTime
        WHERE
            shift_id = @ShiftId
    ";
    public static readonly string DeleteShift = $@"
        DELETE FROM {Schema}.shift
        WHERE 
            shift_id = @ShiftId
    ";
}