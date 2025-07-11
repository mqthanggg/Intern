public static class StaffQuery
{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SelectStaff = $@"
        SELECT 
            staff_id,
            staff_name,
            date_birth,
            phone,
            address,
            email
        FROM {Schema}.staff
        ORDER BY
            staff_id
    ";
    public static readonly string SelectStaffById = $@"
        SELECT 
            staff_id,
            staff_name,
            date_birth,
            phone,
            address,
            email
        FROM {Schema}.staff
        WHERE
            staff_id = @StaffId
    ";
    public static readonly string InsertStaff = $@"
        INSERT INTO {Schema}.staff(
            staff_name,
            date_birth,
            phone,
            address,
            email,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @StaffName,
            @DateBirth,
            @Phone,
            @Address,
            @Email,
            @CreatedBy,
            now(),
            @LastModifiedBy,
            now()
        )
    ";
    public static readonly string UpdateStaff = $@"
        UPDATE {Schema}.staff
        SET
            staff_name = @StaffName,
            date_birth = @DateBirth,
            phone =  @Phone,
            address = @Address,
            email = @Email,
            last_modified_by = @LastModifiedBy, 
            last_modified_date = now() 
        WHERE
            staff_id = @StaffId
    ";
    public static readonly string DeleteStaff = $@"
        DELETE FROM {Schema}.staff
        WHERE 
            staff_id = @StaffId
    ";
}