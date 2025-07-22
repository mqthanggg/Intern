public static class AssignmentQuery{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string SelectAssignment = $@"
        SELECT 
            a.assignment_id,
            sh.shift_type,
            st.staff_name,
            a.station_id,
            a.work_date
        FROM {Schema}.assignment as a
        INNER JOIN {Schema}.staff as st
        INNER JOIN {Schema}.shift as sh
        ON
            a.staff_id = st.staff_id
        AND
            a.shift_id = sh.shift_id
        ORDER BY
            a.assignment_id
    ";
    public static readonly string SelectAssignmentById = $@"
        SELECT 
            a.assignment_id,
            sh.shift_type,
            st.staff_name,
            a.station_id,
            a.work_date
        FROM {Schema}.assignment as a
        INNER JOIN {Schema}.staff as st
        INNER JOIN {Schema}.shift as sh
        ON
            a.staff_id = st.staff_id
        AND
            a.shift_id = sh.shift_id
        ORDER BY
            a.assignment_id
        WHERE
            a.assignment_id = @AssignmentId
    ";
    public static readonly string InsertAssignment = $@"
        INSERT INTO {Schema}.assignment(
            shift_id,
            staff_id,
            station_id,
            work_date,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @ShiftId,
            @StaffId,
            @StationId,
            @WorkDate,
            @CreatedBy,
            now(),
            @LastModifiedBy,
            now()
        )
    ";
    public static readonly string UpdateAssignment = $@"
        UPDATE {Schema}.assignment
        SET
            shift_id = @ShiftId,
            staff_id = @StaffId,
            station_id = @StationId,
            work_date = @WorkDate,
            last_modified_by = @LastModifiedBy,
            last_modified_date = now()
        WHERE
            assignment_id = @AssignmentId
    ";
    public static readonly string DeleteAssignment = $@"
        DELETE FROM {Schema}.assignment
        WHERE 
            assignment_id = @AssignmentId
    ";
    public static readonly string SelectAssignmentByStationId = $@"
        SELECT 
            a.assignment_id,
            sh.shift_type,
            st.staff_name,
            a.station_id,
            a.work_date
        FROM {Schema}.assignment as a
        INNER JOIN {Schema}.staff as st
        INNER JOIN {Schema}.shift as sh
        ON
            a.staff_id = st.staff_id
        AND
            a.shift_id = sh.shift_id
        ORDER BY
            a.assignment_id
        WHERE
            a.station_id = @StationId
    ";
}