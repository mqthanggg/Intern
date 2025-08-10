public static class LogController
{
    public static WebApplication MapLogController(this WebApplication app)
    {
        app.MapGet("log/dispenser/{id}/{name}", GetLogByDispenserName);
        app.MapGet("log/fuel/{id}/{name}", GetLogByFuelName);
        app.MapGet("log/type/{id}/{type}", GetLogByLogType);
        app.MapGet("log/date/{id}/{date}", GetLogByDate);
        app.MapGet("log/period/{id}/{fromDate}/{toDate}", GetLogByPeriod);
        
        app.MapGet("log/period/dispenser/fuel/{id}/{fromDate}/{toDate}/{name}/{fuelName}", GetLogByPeriodDispenerFuel);
        app.MapGet("log/period/dispenser/log/{id}/{fromDate}/{toDate}/{name}/{logType}", GetLogByPeriodDispenerLogType);
        app.MapGet("log/period/fuel/log/{id}/{fromDate}/{toDate}/{fuelName}/{logType}", GetLogByPeriodFuelLogType);
        app.MapGet("log/dispenser/fuel/log/{id}/{name}/{fuelName}/{logType}", GetLogByDipenserFuelLog);

        app.MapGet("log/full/condition/{id}/{fromDate}/{toDate}/{name}/{fuelName}/{logType}", GetLogByFullConditions);     
        return app;
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve dispenser log by station id.",
        Description = "Retrieve a list of logs that are related to the dispenser name belong to the given station."
    )]
    public static async Task<IResult> GetLogByDispenserName([FromRoute] int id, [FromRoute] int name,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenserNameAsync(new GetDispenserResponse { StationId = id, Name = name }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }

    //================================
    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve fuelName log by station id.",
        Description = "Retrieve a list of logs that are related to the fuel name belong to the given station."
    )]
    public static async Task<IResult> GetLogByFuelName([FromRoute] int id, [FromServices] ILogRepository logRepository,
        string name, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelNameAsync(new GetFuelResponse { StationId = id, FuelName = name }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve type log by station id.",
        Description = "Retrieve a list of logs that are related to the log type belong to the given station."
    )]
    public static async Task<IResult> GetLogByLogType([FromRoute] int id, [FromServices] ILogRepository logRepository, int type,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByLogTypeAsync(new GetLogTypeResponse { StationId = id, LogType = type }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }

    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get day",
        Description = "Total revenue statistics for the day by name"
    )]
    public static async Task<IResult> GetLogByDate([FromRoute] int id, [FromRoute] DateTime date,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDateAsync(new GetDateRevenue { StationId = id, Time = date }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }

    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get period",
        Description = "Total revenue statistics for the period by station id"
    )]
    public static async Task<IResult> GetLogByPeriod([FromRoute] int id, [FromRoute] DateTime ToDate, [FromRoute] DateTime FromDate,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodAsync(new GetPeriodResponse { StationId = id, ToDate = ToDate, FromDate = FromDate }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }

    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get period, dispenser, fuel",
        Description = "Total revenue statistics for the period, dispenser and fuel by station id"
    )]
    public static async Task<IResult> GetLogByPeriodDispenerFuel([FromRoute] int id, [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate,
        [FromRoute] int Name, [FromRoute] string FuelName, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodAsync(
            new GetPeriodDispenerFuelResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, Name = Name, FuelName = FuelName }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }

    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get period, dispenser, log type",
        Description = "Total revenue statistics for the period, dispenser and log type by station id"
    )]
    public static async Task<IResult> GetLogByPeriodDispenerLogType([FromRoute] int id, [FromRoute] DateTime ToDate, [FromRoute] DateTime FromDate,
    [FromRoute] int Name, [FromRoute] int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodDispenerLogAsync(
            new GetPeriodDispenerLogResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, Name = Name, LogType = LogType }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }

    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get period, fuel, log type",
        Description = "Total revenue statistics for the period, fuel name and log type by station id"
    )]
    public static async Task<IResult> GetLogByPeriodFuelLogType([FromRoute] int id, [FromRoute] DateTime ToDate, [FromRoute] DateTime FromDate,
    string FuelName, int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodLogFuelAsync(
            new GetPeriodFuelLogResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, FuelName = FuelName, LogType = LogType }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }

    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get dispenser, fuel, log type",
        Description = "Total revenue statistics for dispenser name, fuel name and log type by station id"
    )]
    public static async Task<IResult> GetLogByDipenserFuelLog([FromRoute] int id, [FromRoute] int Name, [FromRoute] string FuelName, [FromRoute] int LogType,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetDispenserFuelLogAsync(
            new GetDipenserFuelLogResponse { StationId = id, Name = Name, FuelName = FuelName, LogType = LogType }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }
    
    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get full condition: dispenser name, period, fuel, log type",
        Description = "Total revenue statistics for the period, dispenser name, fuel name and log type by station id"
    )]
    public static async Task<IResult> GetLogByFullConditions([FromRoute] int id, [FromRoute] DateTime ToDate, [FromRoute] DateTime FromDate, [FromRoute] int Name,
        string FuelName, int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetFullConditionAsync(
            new GetFullConditionResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, Name = Name, FuelName = FuelName, LogType = LogType }, page, pageSize);
        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Data = logs
        };
        return TypedResults.Ok(result);
    }
}