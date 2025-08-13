public class LogRepository : ILogRepository
{
    private readonly IDbWriteConnection dbWrite;
    private readonly IDbReadConnection dbRead;
    private readonly IUsernameService username;
    public LogRepository(
        IDbWriteConnection dbWriteConnection,
        IDbReadConnection dbReadConnection,
        IUsernameService usernameService
    )
    {
        dbWrite = dbWriteConnection;
        dbRead = dbReadConnection;
        username = usernameService;
    }
    public async Task<int> InsertAsync(Log entity)
    {
        entity.CreatedBy ??= username.GetUsername();
        entity.LastModifiedBy ??= username.GetUsername();
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(LogQuery.InsertLog, entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateAsync(Log entity)
    {
        entity.LastModifiedBy ??= username.GetUsername();
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(LogQuery.UpdateLog, entity);
            return affectedRows;
        }
    }
    public async Task<int> DeleteAsync(Log entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(LogQuery.DeleteLog, entity);
            return affectedRows;
        }
    }
    public async Task<int> UpdateLogTimeAsync(LogResponse entity)
    {
        await using (var connection = dbWrite.CreateConnection())
        {
            int affectedRows = await connection.ExecuteAsync(LogQuery.UpdateLogTime, entity);
            return affectedRows;
        }
    }
    public async Task<IReadOnlyList<LogResponse>> GetLogByStationIdAsync(Station entity)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<LogResponse> logs = (await connection.QueryAsync<LogResponse>(LogQuery.SelectLogByStationId, entity)).ToList();
            return logs;
        }
    }
    public async Task<IReadOnlyList<LogResponse>> GetFullLogByStationIdAsync(Station entity)
    {
        await using (var connection = dbRead.CreateConnection())
        {
            List<LogResponse> logs = (await connection.QueryAsync<LogResponse>(LogQuery.SelectFullLogByStationId, entity)).ToList();
            return logs;
        }
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetPageLogByStationIdAsync(Station entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPageLogByStationId, new
            {
                StationId = entity.StationId,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });

        return (logs.ToList(), totalCount);
    }
    // ========================= 1 CONDITION =========================
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserNameAsync(GetDispenserResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserNameByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });

        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelNameAsync(GetFuelResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectFuelNameByStationId, new
            {
                StationId = entity.StationId,
                FuelName = entity.FuelName,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypeAsync(GetLogTypeResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogTypeByStationId, new
            {
                StationId = entity.StationId,
                LogType = entity.LogType,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodAsync(GetPeriodResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodByStationId, new
            {
                StationId = entity.StationId,
                toDate = entity.ToDate.Date,
                fromDate = entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPriceAsync(GetPriceLiterResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogByPriceByStationId, new
            {
                StationId = entity.StationId,
                To = entity.To,
                From = entity.From,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByTotalAmountAsync(GetPriceLiterResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogByTotalAmountByStationId, new
            {
                StationId = entity.StationId,
                ToTotal = entity.To,
                FromTotal = entity.From,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByTotalLiterAsync(GetPriceLiterResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogByTotalLitersByStationId, new
            {
                StationId = entity.StationId,
                To = entity.To,
                From = entity.From,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    //============================ 2 CONDITIONS ============================
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenerFuelAsync(GetDispenerFuelResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectDispenerFuelByStationId, new
        {
            StationId = entity.StationId,
            Name = entity.Name,
            FuelName = entity.FuelName,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenerLogAsync(GetDispenerLogResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectDispenserLogByStationId, new
        {
            StationId = entity.StationId,
            Name = entity.Name,
            LogType = entity.LogType,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerAsync(GetPeriodDispenserResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodDispenerByStationId, new
        {
            StationId = entity.StationId,
            Name = entity.Name,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodFuelAsync(GetPeriodFuelResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodFuelByStationId, new
        {
            StationId = entity.StationId,
            FuelName = entity.FuelName,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogAsync(GetPeriodLogResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodLogByStationId, new
        {
            StationId = entity.StationId,
            LogType = entity.LogType,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelLogAsync(GetFuelLogResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectFuelLogByStationId, new
        {
            StationId = entity.StationId,
            FuelName = entity.FuelName,
            LogType = entity.LogType,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserPriceAsync(GetDipenserPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserPriceByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                From = entity.From,
                To = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserTotalLiterAsync(GetDipenserPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserTotalLitersByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                From = entity.From,
                To = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserTotalAmountAsync(GetDipenserPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserTotalAmountByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                FromTotal = entity.From,
                ToTotal = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelPriceAsync(GetFuelPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectFuelNamePriceByStationId, new
            {
                StationId = entity.StationId,
                FuelName = entity.FuelName,
                From = entity.From,
                To = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelNameTotalLiterAsync(GetFuelPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectFuelNameTotalLitersByStationId, new
            {
                StationId = entity.StationId,
                FuelName = entity.FuelName,
                From = entity.From,
                To = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelNameTotalAmountAsync(GetFuelPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectFuelNameTotalAmountByStationId, new
            {
                StationId = entity.StationId,
                FuelName = entity.FuelName,
                FromTotal = entity.From,
                ToTotal = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypePriceAsync(GeLogTypePriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogPriceByStationId, new
            {
                StationId = entity.StationId,
                LogType = entity.LogType,
                From = entity.From,
                To = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypeTotalLiterAsync(GeLogTypePriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogTotalLitersByStationId, new
            {
                StationId = entity.StationId,
                LogType = entity.LogType,
                From = entity.From,
                To = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypeTotalAmountAsync(GeLogTypePriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogTotalAmountByStationId, new
            {
                StationId = entity.StationId,
                LogType = entity.LogType,
                FromTotal = entity.From,
                ToTotal = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodPriceAsync(GetPeriodPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodPriceByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                ToDate = entity.ToDate.Date,
                FromDate = entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodTotalLiterAsync(GetPeriodPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodTotalLiterByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                toDate = entity.ToDate.Date,
                fromDate = entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodTotalAmountAsync(GetPeriodPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodTotalAmountByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                toDate = entity.ToDate.Date,
                fromDate = entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPriceTotalLiterAsync(GetPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogByPriceTotalLiterByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPriceTotalAmountAsync(GetPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogByPriceTotalAmountByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByTotalLitersTotalAmountAsync(GetPriceLitterAmountResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogByTotalLiterAmountByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    // ============================ 3 CONDITIONS ============================
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerFuelAsync(GetPeriodDispenerFuelResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodDispenerFuelByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                FuelName = entity.FuelName,
                toDate = entity.ToDate.Date,
                fromDate = entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerLogAsync(GetPeriodDispenerLogResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodDispenserLogByStationId, new
            {
                StationId = entity.StationId,
                LogType = entity.LogType,
                Name = entity.Name,
                toDate = entity.ToDate.Date,
                fromDate = entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogFuelAsync(GetPeriodFuelLogResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodFuelLogByStationId, new
            {
                StationId = entity.StationId,
                FuelName = entity.FuelName,
                LogType = entity.LogType,
                toDate = entity.ToDate.Date,
                fromDate = entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserFuelLogAsync(GetDipenserFuelLogResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDipenserFuelLogByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                FuelName = entity.FuelName,
                LogType = entity.LogType,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserFuelPriceAsync(GetDipenserFuelPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserFuelPriceByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                FuelName = entity.FuelName,
                From = entity.From,
                To = entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize,
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserFuelAmountAsync(GetDipenserFuelPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserFuelAmountByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                FuelName = entity.FuelName,
                From=entity.From,
                To=entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserFuelLitersAsync(GetDipenserFuelPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserFuelLitersByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                FuelName = entity.FuelName,
                From=entity.From,
                To=entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserLogPriceAsync(GetDipenserLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserLogPriceByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                LogType=entity.LogType,
                From=entity.From,
                To=entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserLogAmountAsync(GetDipenserLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserLogAmountByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                LogType=entity.LogType,
                From=entity.From,
                To=entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
     public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserLogLitersAsync(GetDipenserLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserLogLitersByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                LogType=entity.LogType,
                From=entity.From,
                To=entity.To,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerPriceAsync(GetPeriodDipenserPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectDispenserPeriodPriceByStationId, new
        {
            StationId = entity.StationId,
            Name = entity.Name,
            From = entity.From,
            To=entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerLitersAsync(GetPeriodDipenserPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectDispenserPeriodLitersByStationId, new
        {
            StationId = entity.StationId,
            Name = entity.Name,
            From=entity.From,
            To=entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerAmountAsync(GetPeriodDipenserPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectDispenserPeriodAmountByStationId, new
        {
            StationId = entity.StationId,
            Name = entity.Name,
            From=entity.From,
            To=entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserPriceAmountAsync(GetDipenserPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserPriceAmountByStationId, new
            {
                StationId = entity.StationId,
                Name=entity.Name,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserPriceLitersAsync(GetDipenserPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserPriceLitersByStationId, new
            {
                StationId = entity.StationId,
                Name=entity.Name,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserAmountLitersAsync(GetDipenserPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserAmountLitersByStationId, new
            {
                StationId = entity.StationId,
                Name=entity.Name,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelLogPriceAsync(GetFuelLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectFuelLogPriceByStationId, new
        {
            StationId = entity.StationId,
            FuelName = entity.FuelName,
            LogType = entity.LogType,
            From = entity.From,
            To = entity.To,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelLogAmountAsync(GetFuelLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectFuelLogAmountByStationId, new
        {
            StationId = entity.StationId,
            FuelName = entity.FuelName,
            LogType = entity.LogType,
            From = entity.From,
            To = entity.To,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelLogLitersAsync(GetFuelLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectFuelLogLitersByStationId, new
        {
            StationId = entity.StationId,
            FuelName = entity.FuelName,
            LogType = entity.LogType,
            From = entity.From,
            To = entity.To,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodFuelPriceAsync(GetPeriodFuelPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodFuelPriceByStationId, new
        {
            StationId = entity.StationId,
            FuelName = entity.FuelName,
            From = entity.From,
            To = entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodFuelAmountAsync(GetPeriodFuelPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodFuelAmountByStationId, new
        {
            StationId = entity.StationId,
            FuelName = entity.FuelName,
            From = entity.From,
            To = entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodFuelLiterAsync(GetPeriodFuelPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodFuelLitersByStationId, new
        {
            StationId = entity.StationId,
            FuelName = entity.FuelName,
            From = entity.From,
            To = entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogFuelPriceLiterAsync(GetFuelPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectFuelPriceLiterByStationId, new
            {
                StationId = entity.StationId,
                FuelName=entity.FuelName,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelPriceAmountAsync(GetFuelPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectFuelPriceAmountByStationId, new
            {
                StationId = entity.StationId,
                FuelName=entity.FuelName,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelAmountLiterAsync(GetFuelPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectFuelAmountLitersByStationId, new
            {
                StationId = entity.StationId,
                FuelName=entity.FuelName,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogPriceAsync(GetPeriodLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodLogPriceByStationId, new
        {
            StationId = entity.StationId,
            LogType = entity.LogType,
            From = entity.From,
            To = entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogAmountAsync(GetPeriodLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodLogAmountByStationId, new
        {
            StationId = entity.StationId,
            LogType = entity.LogType,
            From = entity.From,
            To = entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogLitersAsync(GetPeriodLogPriceResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(LogQuery.SelectPeriodLogLitersByStationId, new
        {
            StationId = entity.StationId,
            LogType = entity.LogType,
            From = entity.From,
            To = entity.To,
            toDate = entity.ToDate.Date,
            fromDate = entity.FromDate.Date,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogPriceAmountAsync(GetLogPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogPriceAmountByStationId, new
            {
                StationId = entity.StationId,
                LogType=entity.LogType,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogPriceLitersAsync(GetLogPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogPriceLitersByStationId, new
            {
                StationId = entity.StationId,
                LogType=entity.LogType,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogAmountLitersAsync(GetLogPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectLogAmountLitersByStationId, new
            {
                StationId = entity.StationId,
                LogType=entity.LogType,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodPriceAmountAsync(GetPeriodPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodPriceAmountByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodPriceLitersAsync(GetPeriodPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPeriodPriceLitersByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodAmountLitersAsync(GetPeriodPriceTotalResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserAmountLitersByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPriceAmountLitersAsync(GetPriceAmountLitersResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(LogQuery.CountLogByStationId, new { entity.StationId });

        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectPriceAmountLitersByStationId, new
            {
                StationId = entity.StationId,
                From = entity.From,
                To = entity.To,
                FromTotal = entity.FromTotal,
                ToTotal = entity.ToTotal,
                FromAmount =entity.FromAmount,
                ToAmount = entity.ToAmount,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }
    // ============================ 4 CONDITION ============================
    public async Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetFullConditionAsync(GetFullConditionResponse entity, int page, int pageSize)
    {
        await using var connection = dbRead.CreateConnection();
        var totalCount = await connection.ExecuteScalarAsync<int>(
            LogQuery.CountLogByStationId, new { entity.StationId });
        var logs = await connection.QueryAsync<LogResponse>(
            LogQuery.SelectDispenserLogFuelPeriodByStationId, new
            {
                StationId = entity.StationId,
                Name = entity.Name,
                FuelName = entity.FuelName,
                LogType = entity.LogType,
                toDate = entity.ToDate.Date,
                fromDate = entity.FromDate.Date,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            });
        return (logs.ToList(), totalCount);
    }

}
