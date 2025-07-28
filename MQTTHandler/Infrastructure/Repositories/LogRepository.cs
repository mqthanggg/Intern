public class LogRepository : ILogRepository{
    private readonly IDbWriteConnection _write;
    public LogRepository(
        IDbWriteConnection write
    ){
        _write = write;
    }
    public async Task<int> InsertAsync(Log entity){
        await using (var connection = _write.CreateConnection()){
            int affectedRows = await connection.ExecuteAsync(
                $@"
                    INSERT INTO {Env.GetString("SCHEMA")}.log(
                        dispenser_id,
                        fuel_name,
                        total_liters,
                        total_amount,
                        time,
                        log_type,
                        created_by,
                        created_date,
                        last_modified_by,
                        last_modified_date
                    ) VALUES (
                        @DispenserId,
                        (
                            SELECT short_name 
                            FROM {Env.GetString("SCHEMA")}.fuel 
                            WHERE 
                            fuel_id = (
                                SELECT fuel_id 
                                FROM {Env.GetString("SCHEMA")}.dispenser
                                WHERE
                                    dispenser_id = @DispenserId
                                LIMIT 1
                            )
                            LIMIT 1
                        ),
                        @TotalLiters,
                        @TotalAmount,
                        now(),
                        @LogType,
                        @CreatedBy,
                        now(),
                        @LastModifiedBy,
                        now()
                    )
                ",
                entity
            );
            return affectedRows;
        }
    }
}