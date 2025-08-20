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
                    ) SELECT 
                        @DispenserId,
                        f.short_name,
                        @TotalLiters,
                        @TotalAmount,
                        LOCALTIMESTAMP(0),
                        @LogType,
                        @CreatedBy,
                        now(),
                        @LastModifiedBy,
                        now()
                    FROM {Env.GetString("SCHEMA")}.dispenser d
                    JOIN {Env.GetString("SCHEMA")}.fuel f 
                    ON 
                        d.fuel_id = f.fuel_id
                    WHERE 
                        d.dispenser_id = @DispenserId
                ",
                entity
            );
            return affectedRows;
        }
    }
}