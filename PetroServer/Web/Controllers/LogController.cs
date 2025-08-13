public static class LogController
{
    public static WebApplication MapLogController(this WebApplication app)
    {
        // ============================ 1 CONDITIONS ============================
        app.MapGet("log/dispenser/{id}/{name}", GetLogByDispenserName);
        app.MapGet("log/fuel/{id}/{name}", GetLogByFuelName);
        app.MapGet("log/type/{id}/{type}", GetLogByLogType);
        app.MapGet("log/period/{id}/{fromDate}/{toDate}", GetLogByPeriod);
        app.MapGet("log/price/{id}/{fromPrice}/{toPrice}", GetLogByPrice);
        app.MapGet("log/total/liter/{id}/{fromTotal}/{toTotal}", GetLogByTotalLiters);
        app.MapGet("log/total/amount/{id}/{fromTotal}/{toTotal}", GetLogByTotalAmount);
    // ============================ 2 CONDITIONS ============================
        app.MapGet("log/dispener/fuel/{id}/{name}/{fuelName}", GetLogByDispenserFuel);
        app.MapGet("log/dispener/log/{id}/{name}/{logType}", GetLogByDispenserLog);
        app.MapGet("log/period/dispenser/{id}/{fromDate}/{toDate}/{name}", GetLogByPeriodDispenser);
        app.MapGet("log/period/fuel/{id}/{fromDate}/{toDate}/{fuelName}", GetLogByPeriodFuel);
        app.MapGet("log/period/log/{id}/{fromDate}/{toDate}/{logType}", GetLogByPeriodLog);
        app.MapGet("log/fuel/log/{id}/{fuelName}/{logType}", GetLogByFuelLog);
        app.MapGet("log/dispener/price/{id}/{name}/{from}/{to}", GetLogByDispenserPrice);
        app.MapGet("log/dispener/liter/{id}/{name}/{from}/{to}", GetLogByDispenserTotalLiter);
        app.MapGet("log/dispener/amount/{id}/{name}/{from}/{to}", GetLogByDispenserTotalAmount);
        app.MapGet("log/fuel/price/{id}/{fuelName}/{from}/{to}", GetLogByFuelNamePrice);
        app.MapGet("log/fuel/liter/{id}/{fuelName}/{from}/{to}", GetLogByFuelNameTotalLiter); 
        app.MapGet("log/fuel/amount/{id}/{fuelName}/{from}/{to}", GetLogByFuelNameTotalAmount);
        app.MapGet("log/type/price/{id}/{logType}/{from}/{to}", GetLogByLogTypePrice);
        app.MapGet("log/type/liter/{id}/{logType}/{from}/{to}", GetLogByLogTypeTotalLiter);
        app.MapGet("log/type/amount/{id}/{logType}/{from}/{to}", GetLogByLogTypeTotalAmount);        
        app.MapGet("log/period/price/{id}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodPrice);
        app.MapGet("log/period/liter/{id}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodTotalLiter);
        app.MapGet("log/period/amount/{id}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodTotalAmount); 
        app.MapGet("log/price/amount/{id}/{fromTotal}/{toTotal}/{from}/{to}", GetLogByPriceTotalAmount);
        app.MapGet("log/price/liter/{id}/{fromTotal}/{toTotal}/{from}/{to}", GetLogByPriceTotalLiter); 
        app.MapGet("log/liter/amount/{id}/{fromTotal}/{toTotal}/{from}/{to}", GetLogByTotalLitersTotalAmount);
        // ============================ 3 CONDITIONS ============================
        app.MapGet("log/period/dispenser/fuel/{id}/{fromDate}/{toDate}/{name}/{fuelName}", GetLogByPeriodDispenerFuel);  
        app.MapGet("log/period/dispenser/log/{id}/{fromDate}/{toDate}/{name}/{logType}", GetLogByPeriodDispenerLogType);
        app.MapGet("log/period/fuel/log/{id}/{fromDate}/{toDate}/{fuelName}/{logType}", GetLogByPeriodFuelLogType);
        app.MapGet("log/dispenser/fuel/log/{id}/{name}/{fuelName}/{logType}", GetLogByDipenserFuelLog);
        app.MapGet("log/dispenser/fuel/price/{id}/{name}/{fuelName}/{from}/{to}", GetLogByDipenserFuelPrice);  
        app.MapGet("log/dispenser/fuel/amount/{id}/{name}/{fuelName}/{from}/{to}", GetLogByDipenserFuelAmount);
        app.MapGet("log/dispenser/fuel/liter/{id}/{name}/{fuelName}/{from}/{to}", GetLogByDipenserFuelLiters);  
        app.MapGet("log/dispenser/log/price/{id}/{name}/{logType}/{from}/{to}", GetLogByDipenserLogPrice);
        app.MapGet("log/dispenser/log/liter/{id}/{name}/{logType}/{from}/{to}", GetLogByDipenserLogLiter);  
        app.MapGet("log/dispenser/log/amount/{id}/{name}/{logType}/{from}/{to}", GetLogByDipenserLogAmount);
        app.MapGet("log/period/dispenser/price/{id}/{name}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodDispenserPrice);
        app.MapGet("log/period/dispenser/amount/{id}/{name}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodDispenserAmount);
        app.MapGet("log/period/dispenser/liter/{id}/{name}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodDispenserLiters);
        app.MapGet("log/dispenser/price/amount/{id}/{name}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByDispenserPriceAmount);
        app.MapGet("log/dispenser/price/liter/{id}/{name}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByDispenserPriceLiters);
        app.MapGet("log/dispenser/liter/amount/{id}/{name}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByDispenserLitersAmount);  
        app.MapGet("log/fuel/logType/price/{id}/{fuelName}/{logType}/{From}/{To}", GetLogByFuelLogPrice);  
        app.MapGet("log/fuel/logType/amount/{id}/{fuelName}/{logType}/{From}/{To}", GetLogByFuelLogAmount);
        app.MapGet("log/fuel/logType/liter/{id}/{fuelName}/{logType}/{From}/{To}", GetLogByFuelLogLiters);
        app.MapGet("log/period/fuel/price/{id}/{fuelName}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodFuelPrice);
        app.MapGet("log/period/fuel/amount/{id}/{fuelName}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodFuelAmount);
        app.MapGet("log/period/fuel/liter/{id}/{fuelName}/{fromDate}/{toDate}/{from}/{to}", GetLogByPeriodFuelLiters);  
        app.MapGet("log/fuel/price/liter/{id}/{fuelName}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByFuelNamePriceLiter);  
        app.MapGet("log/fuel/price/amount/{id}/{fuelName}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByFuelNamePriceAmount);
        app.MapGet("log/fuel/amount/liter/{id}/{fuelName}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByFuelNameAmountLiter); 
        app.MapGet("log/period/log/price/{id}/{fromDate}/{toDate}/{logType}/{from}/{to}", GetLogByPeriodLogPrice);
        app.MapGet("log/period/log/amount/{id}/{fromDate}/{toDate}/{logType}/{from}/{to}", GetLogByPeriodLogAmount);
        app.MapGet("log/period/log/liter/{id}/{fromDate}/{toDate}/{logType}/{from}/{to}", GetLogByPeriodLogLiter);
        app.MapGet("log/logType/price/amount/{id}/{logType}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByLogTypePriceAmount);
        app.MapGet("log/logType/price/liter/{id}/{logType}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByLogTypePriceLiter);
        app.MapGet("log/logType/amount/liter/{id}/{logType}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByLogTypeAmountLiter);
        app.MapGet("log/period/price/liter/{id}/{fromDate}/{toDate}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByPeriodPriceLiters);
        app.MapGet("log/period/price/amount/{id}/{fromDate}/{toDate}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByPeriodPriceAmount);
        app.MapGet("log/period/amount/liter/{id}/{fromDate}/{toDate}/{from}/{to}/{FromTotal}/{ToTotal}", GetLogByPeriodAmountLiters);
        app.MapGet("log/price/amount/liter/{id}/{from}/{to}/{fromAmount}/{toAmount}/{FromTotal}/{ToTotal}", GetLogByPriceAmountLiters);

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

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve fuelName log by station id.",
        Description = "Retrieve a list of logs that are related to the fuel name belong to the given station."
    )]
    public static async Task<IResult> GetLogByFuelName([FromRoute] int id, [FromServices] ILogRepository logRepository, 
       [FromRoute] string name, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
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
        Summary = "Report log get period",
        Description = "Total revenue statistics for the period by station id"
    )]
    public static async Task<IResult> GetLogByPeriod([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate,
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
        Summary = "Report log get price",
        Description = "Total revenue statistics for price by station id"
    )]
    public static async Task<IResult> GetLogByPrice([FromRoute] int id,[FromRoute] int FromPrice, [FromRoute] int ToPrice, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPriceAsync(
            new GetPriceLiterResponse { StationId = id, From = FromPrice, To = ToPrice }, page, pageSize);
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
        Summary = "Report log get total liter",
        Description = "Total revenue statistics for total liter by station id"
    )]
    public static async Task<IResult> GetLogByTotalLiters([FromRoute] int id, [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByTotalLiterAsync(
            new GetPriceLiterResponse { StationId = id,  From=FromTotal, To=ToTotal }, page, pageSize);
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
        Summary = "Report log get total amount",
        Description = "Total revenue statistics for total amount by station id"
    )]
    public static async Task<IResult> GetLogByTotalAmount([FromRoute] int id, [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByTotalAmountAsync(
            new GetPriceLiterResponse { StationId = id,  From=FromTotal, To=ToTotal }, page, pageSize);
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
        Summary = "Report log get dispenser, fuel",
        Description = "Total revenue statistics for dispenser name, fuel name by station id"
    )]
    public static async Task<IResult> GetLogByDispenserFuel([FromRoute] int id, [FromRoute] int Name,
       [FromRoute] string FuelName, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenerFuelAsync(
            new GetDispenerFuelResponse { StationId = id, Name = Name, FuelName = FuelName }, page, pageSize);
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
        Summary = "Report log get dispenser name, log type",
        Description = "Total revenue statistics for dispenser name log type by station id"
    )]
    public static async Task<IResult> GetLogByDispenserLog([FromRoute] int id, [FromRoute] int Name,
       [FromRoute] int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenerLogAsync(
            new GetDispenerLogResponse { StationId = id, Name = Name, LogType = LogType }, page, pageSize);
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
        Summary = "Report log get period, dispenser name",
        Description = "Total revenue statistics for the period, dispenser name by station id"
    )]
    public static async Task<IResult> GetLogByPeriodDispenser([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int Name,
       [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodDispenerAsync(
            new GetPeriodDispenserResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, Name = Name }, page, pageSize);
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
        Summary = "Report log get period, fuel",
        Description = "Total revenue statistics for the periode, fuel name by station id"
    )]
    public static async Task<IResult> GetLogByPeriodFuel ([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate,
       [FromRoute] string FuelName, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodFuelAsync(
            new GetPeriodFuelResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, FuelName = FuelName }, page, pageSize);
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
        Summary = "Report log get period, log type",
        Description = "Total revenue statistics for the period, log type by station id"
    )]
    public static async Task<IResult> GetLogByPeriodLog([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate,
        [FromRoute] int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodLogAsync(
            new GetPeriodLogResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, LogType = LogType }, page, pageSize);
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
        Summary = "Report log get fuel, log type",
        Description = "Total revenue statistics for fuel name, log type by station id"
    )]
    public static async Task<IResult> GetLogByFuelLog([FromRoute] int id, [FromRoute] string FuelName, [FromRoute] int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelLogAsync(
            new GetFuelLogResponse { StationId = id,  FuelName = FuelName, LogType = LogType }, page, pageSize);
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
        Summary = "Report log get dispenser and price",
        Description = "Total revenue statistics for dispenser name and price by station id"
    )]
    public static async Task<IResult> GetLogByDispenserPrice([FromRoute] int id, [FromRoute] int name, [FromRoute] int from, [FromRoute] int to,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenserPriceAsync(
            new GetDipenserPriceLitterAmountResponse { StationId = id,  Name=name, From=from, To=to}, page, pageSize);
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
        Summary = "Report log get dispense, total liter",
        Description = "Total revenue statistics for dispenser name and total liter by station id"
    )]
    public static async Task<IResult> GetLogByDispenserTotalLiter([FromRoute] int id, [FromRoute] int name, [FromRoute] int from, [FromRoute] int to,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenserTotalLiterAsync(
            new GetDipenserPriceLitterAmountResponse { StationId = id,  Name=name, From=from, To=to}, page, pageSize);
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
        Summary = "Report log get dispenser, total amount",
        Description = "Total revenue statistics for dispenser name and total amount by station id"
    )]
    public static async Task<IResult> GetLogByDispenserTotalAmount([FromRoute] int id, [FromRoute] int name, [FromRoute] int from, [FromRoute] int to,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenserTotalAmountAsync(
            new GetDipenserPriceLitterAmountResponse { StationId = id, Name=name, From=from, To=to }, page, pageSize);
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
        Summary = "Report log get fuel name and price",
        Description = "Total revenue statistics for fuel name and price by station id"
    )]
    public static async Task<IResult> GetLogByFuelNamePrice([FromRoute] int id, [FromRoute] string fuelName, [FromRoute] int from, [FromRoute] int to,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelPriceAsync(
            new GetFuelPriceLitterAmountResponse { StationId = id, FuelName=fuelName, From=from, To=to}, page, pageSize);
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
        Summary = "Report log get dispense, total liter",
        Description = "Total revenue statistics for dispenser name and total liter by station id"
    )]
    public static async Task<IResult> GetLogByFuelNameTotalLiter([FromRoute] int id, [FromRoute] string fuelName, [FromRoute] int from, [FromRoute] int to,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelNameTotalLiterAsync(
            new GetFuelPriceLitterAmountResponse { StationId = id, FuelName=fuelName, From=from, To=to}, page, pageSize);
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
        Summary = "Report log get fuel name, total amount",
        Description = "Total revenue statistics for fuel name and total amount by station id"
    )]
    public static async Task<IResult> GetLogByFuelNameTotalAmount([FromRoute] int id, [FromRoute] string fuelName, [FromRoute] int From, [FromRoute] int To,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelNameTotalAmountAsync(
            new GetFuelPriceLitterAmountResponse { StationId = id, FuelName=fuelName, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get log type and price",
        Description = "Total revenue statistics for log type and price by station id"
    )]
    public static async Task<IResult> GetLogByLogTypePrice([FromRoute] int id, [FromRoute] int logType, [FromRoute] int From, [FromRoute] int To,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByLogTypePriceAsync(
            new GeLogTypePriceLitterAmountResponse { StationId = id,  LogType=logType, From=From, To=To}, page, pageSize);
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
        Summary = "Report log get log type, total liter",
        Description = "Total revenue statistics for log type and total liter by station id"
    )]
    public static async Task<IResult> GetLogByLogTypeTotalLiter([FromRoute] int id, [FromRoute] int logType, [FromRoute] int From, [FromRoute] int To,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByLogTypeTotalLiterAsync(
            new GeLogTypePriceLitterAmountResponse { StationId = id,  LogType=logType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get log type, total amount",
        Description = "Total revenue statistics for log type and total amount by station id"
    )]
    public static async Task<IResult> GetLogByLogTypeTotalAmount([FromRoute] int id, [FromRoute] int logType, [FromRoute] int From, [FromRoute] int To,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByLogTypeTotalAmountAsync(
            new GeLogTypePriceLitterAmountResponse { StationId = id, LogType=logType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get period, price",
        Description = "Total revenue statistics for the period, price by station id"
    )]
    public static async Task<IResult> GetLogByPeriodPrice([FromRoute] int id, [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodPriceAsync(new GetPeriodPriceLitterAmountResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get period, total liter",
        Description = "Total revenue statistics for the period and total liter by station id"
    )]
    public static async Task<IResult> GetLogByPeriodTotalLiter([FromRoute] int id, [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodTotalLiterAsync(new GetPeriodPriceLitterAmountResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, From=From, To=To }, page, pageSize);
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
    public static async Task<IResult> GetLogByPeriodTotalAmount([FromRoute] int id, [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodTotalAmountAsync(new GetPeriodPriceLitterAmountResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, From = From, To=To }, page, pageSize);
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
    public static async Task<IResult> GetLogByPriceTotalAmount([FromRoute] int id, [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromRoute] int From, [FromRoute] int To,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPriceTotalAmountAsync(new GetPriceLitterAmountResponse { StationId = id, ToTotal = ToTotal, FromTotal = FromTotal, From = From, To=To }, page, pageSize);
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
    public static async Task<IResult> GetLogByPriceTotalLiter([FromRoute] int id, [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromRoute] int From, [FromRoute] int To,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPriceTotalLiterAsync(new GetPriceLitterAmountResponse { StationId = id, ToTotal = ToTotal, FromTotal = FromTotal, From = From, To=To }, page, pageSize);
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
    public static async Task<IResult> GetLogByTotalLitersTotalAmount([FromRoute] int id, [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromRoute] int From, [FromRoute] int To,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByTotalLitersTotalAmountAsync(new GetPriceLitterAmountResponse { StationId = id, ToTotal=ToTotal, FromTotal = FromTotal, From = From, To=To }, page, pageSize);
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
    //===========================================================
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
        var (logs, totalCount) = await logRepository.GetLogByPeriodDispenerFuelAsync(
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
    public static async Task<IResult> GetLogByPeriodDispenerLogType([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate,
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
    public static async Task<IResult> GetLogByPeriodFuelLogType([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate,
       [FromRoute] string FuelName, int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
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
        Summary = "Report log get dispenser, fuel, price",
        Description = "Total revenue statistics for dispenser name, fuel name and price by station id"
    )]
    public static async Task<IResult> GetLogByDipenserFuelPrice([FromRoute] int id, [FromRoute] int Name, [FromRoute] string FuelName, [FromRoute] int From, [FromRoute] int To,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetDispenserFuelPriceAsync(
            new GetDipenserFuelPriceResponse { StationId = id, Name = Name, FuelName = FuelName, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get dispenser, fuel, total amount",
        Description = "Total revenue statistics for dispenser name, fuel name and total amount by station id"
    )]
    public static async Task<IResult> GetLogByDipenserFuelAmount([FromRoute] int id, [FromRoute] int Name, [FromRoute] string FuelName, [FromRoute] int From, [FromRoute] int To,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetDispenserFuelAmountAsync(
            new GetDipenserFuelPriceResponse { StationId = id, Name = Name, FuelName = FuelName, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get dispenser, fuel, total liter",
        Description = "Total revenue statistics for dispenser name, fuel name and total liter by station id"
    )]
    public static async Task<IResult> GetLogByDipenserFuelLiters([FromRoute] int id, [FromRoute] int Name, [FromRoute] string FuelName, [FromRoute] int From, [FromRoute] int To,
        [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetDispenserFuelLitersAsync(
            new GetDipenserFuelPriceResponse { StationId = id, Name = Name, FuelName = FuelName, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get dispenser, log type, price",
        Description = "Total revenue statistics for dispenser name, log type and price by station id"
    )]
    public static async Task<IResult> GetLogByDipenserLogPrice([FromRoute] int id, [FromRoute] int Name, [FromRoute] int LogType,
       [FromRoute] int From, [FromRoute] int To, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetDispenserLogPriceAsync(
            new GetDipenserLogPriceResponse { StationId = id, Name = Name, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get dispenser, log type, total liter",
        Description = "Total revenue statistics for dispenser name, log type and total liter by station id"
    )]
    public static async Task<IResult> GetLogByDipenserLogLiter([FromRoute] int id, [FromRoute] int Name, [FromRoute] int LogType,
       [FromRoute] int From, [FromRoute] int To, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetDispenserLogLitersAsync(
            new GetDipenserLogPriceResponse { StationId = id, Name = Name, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get dispenser, log type, total amount",
        Description = "Total revenue statistics for dispenser name, log type and total amount by station id"
    )]
    public static async Task<IResult> GetLogByDipenserLogAmount([FromRoute] int id, [FromRoute] int Name, [FromRoute] int LogType,
       [FromRoute] int From, [FromRoute] int To, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetDispenserLogAmountAsync(
            new GetDipenserLogPriceResponse { StationId = id, Name = Name, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get period, dispenser name, price",
        Description = "Total revenue statistics for the period, dispenser name and price by station id"
    )]
    public static async Task<IResult> GetLogByPeriodDispenserPrice([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int Name,
      [FromRoute] int From, [FromRoute] int To, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodDispenerPriceAsync(
            new GetPeriodDipenserPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, Name = Name, From = From, To=To }, page, pageSize);
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
        Summary = "Report log get period, dispenser name, total liter",
        Description = "Total revenue statistics for the period, dispenser name and total liter by station id"
    )]
    public static async Task<IResult> GetLogByPeriodDispenserLiters([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int Name,
      [FromRoute] int From, [FromRoute] int To, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodDispenerLitersAsync(
            new GetPeriodDipenserPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, Name = Name, From = From, To=To }, page, pageSize);
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
        Summary = "Report log get period, dispenser name, total amount",
        Description = "Total revenue statistics for the period, dispenser name and total amount by station id"
    )]
    public static async Task<IResult> GetLogByPeriodDispenserAmount([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int Name,
      [FromRoute] int From, [FromRoute] int To, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodDispenerAmountAsync(
            new GetPeriodDipenserPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, Name = Name, From = From, To=To }, page, pageSize);
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
        Summary = "Report log get dispenser and price, total amount",
        Description = "Total revenue statistics for dispenser name and price, total amount by station id"
    )]
    public static async Task<IResult> GetLogByDispenserPriceAmount([FromRoute] int id, [FromRoute] int name, [FromRoute] int from, [FromRoute] int to,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenserPriceAmountAsync(
            new GetDipenserPriceTotalResponse { StationId = id,  Name=name, From=from, To=to, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get dispenser and price, total liter",
        Description = "Total revenue statistics for dispenser name and price, total liter by station id"
    )]
    public static async Task<IResult> GetLogByDispenserPriceLiters([FromRoute] int id, [FromRoute] int name, [FromRoute] int from, [FromRoute] int to,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenserPriceLitersAsync(
            new GetDipenserPriceTotalResponse { StationId = id,  Name=name, From=from, To=to, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get dispenser and total liter, total amount",
        Description = "Total revenue statistics for dispenser name and total liter, total amount by station id"
    )]
    public static async Task<IResult> GetLogByDispenserLitersAmount([FromRoute] int id, [FromRoute] int name, [FromRoute] int from, [FromRoute] int to,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByDispenserAmountLitersAsync(
            new GetDipenserPriceTotalResponse { StationId = id,  Name=name, From=from, To=to, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get fuel, log type, price",
        Description = "Total revenue statistics for fuel name, log type, price by station id"
    )]
    public static async Task<IResult> GetLogByFuelLogPrice([FromRoute] int id, [FromRoute] string FuelName, [FromRoute] int LogType, [FromRoute] int From, [FromRoute] int To,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelLogPriceAsync(
            new GetFuelLogPriceResponse { StationId = id,  FuelName = FuelName, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get fuel, log type, total amount",
        Description = "Total revenue statistics for fuel name, log type, total amount by station id"
    )]
    public static async Task<IResult> GetLogByFuelLogAmount([FromRoute] int id, [FromRoute] string FuelName, [FromRoute] int LogType, [FromRoute] int From, [FromRoute] int To,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelLogAmountAsync(
            new GetFuelLogPriceResponse { StationId = id,  FuelName = FuelName, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get fuel, log type, total liter",
        Description = "Total revenue statistics for fuel name, log type, total liter by station id"
    )]
    public static async Task<IResult> GetLogByFuelLogLiters([FromRoute] int id, [FromRoute] string FuelName, [FromRoute] int LogType, [FromRoute] int From, [FromRoute] int To,
    [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelLogLitersAsync(
            new GetFuelLogPriceResponse { StationId = id,  FuelName = FuelName, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get period, fuel, price",
        Description = "Total revenue statistics for the periode, fuel name, price by station id"
    )]
    public static async Task<IResult> GetLogByPeriodFuelPrice ([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] string FuelName, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodFuelPriceAsync(
            new GetPeriodFuelPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, FuelName = FuelName, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get period, fuel, total amount",
        Description = "Total revenue statistics for the periode, fuel name, total amount by station id"
    )]
    public static async Task<IResult> GetLogByPeriodFuelAmount ([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] string FuelName, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodFuelAmountAsync(
            new GetPeriodFuelPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, FuelName = FuelName, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get period, fuel, total liter",
        Description = "Total revenue statistics for the periode, fuel name, total liter by station id"
    )]
    public static async Task<IResult> GetLogByPeriodFuelLiters ([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] string FuelName, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodFuelLiterAsync(
            new GetPeriodFuelPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, FuelName = FuelName, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get fuel name price, total liter",
        Description = "Total revenue statistics for fuel name, price, total liter by station id"
    )]
    public static async Task<IResult> GetLogByFuelNamePriceLiter([FromRoute] int id, [FromRoute] string fuelName, [FromRoute] int from, [FromRoute] int to,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogFuelPriceLiterAsync(
            new GetFuelPriceTotalResponse { StationId = id, FuelName=fuelName, From=from, To=to, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get fuel name price, total amount",
        Description = "Total revenue statistics for fuel name, price, total amount by station id"
    )]
    public static async Task<IResult> GetLogByFuelNamePriceAmount([FromRoute] int id, [FromRoute] string fuelName, [FromRoute] int from, [FromRoute] int to,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelPriceAmountAsync(
            new GetFuelPriceTotalResponse { StationId = id, FuelName=fuelName, From=from, To=to, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get fuel name, total amount, total liter",
        Description = "Total revenue statistics for fuel name, total amount, total liter by station id"
    )]
    public static async Task<IResult> GetLogByFuelNameAmountLiter([FromRoute] int id, [FromRoute] string fuelName, [FromRoute] int from, [FromRoute] int to,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByFuelAmountLiterAsync(
            new GetFuelPriceTotalResponse { StationId = id, FuelName=fuelName, From=from, To=to, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get period, log type, price",
        Description = "Total revenue statistics for the period, log type, price by station id"
    )]
    public static async Task<IResult> GetLogByPeriodLogPrice([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
        [FromRoute] int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodLogPriceAsync(
            new GetPeriodLogPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get period, log type, total liter",
        Description = "Total revenue statistics for the period, log type, total liter by station id"
    )]
    public static async Task<IResult> GetLogByPeriodLogLiter([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
        [FromRoute] int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodLogLitersAsync(
            new GetPeriodLogPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get period, log type, total amount",
        Description = "Total revenue statistics for the period, log type, total amount by station id"
    )]
    public static async Task<IResult> GetLogByPeriodLogAmount([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
        [FromRoute] int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodLogAmountAsync(
            new GetPeriodLogPriceResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, LogType = LogType, From=From, To=To }, page, pageSize);
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
        Summary = "Report log get log type and price, total amount",
        Description = "Total revenue statistics for log type and price, total amount by station id"
    )]
    public static async Task<IResult> GetLogByLogTypePriceAmount([FromRoute] int id, [FromRoute] int logType, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByLogPriceAmountAsync(
            new GetLogPriceTotalResponse { StationId = id,  LogType=logType, From=From, To=To, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get log type and price, total liter",
        Description = "Total revenue statistics for log type and price, total liter by station id"
    )]
    public static async Task<IResult> GetLogByLogTypePriceLiter([FromRoute] int id, [FromRoute] int logType, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByLogPriceLitersAsync(
            new GetLogPriceTotalResponse { StationId = id,  LogType=logType, From=From, To=To, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get log type and total liter, total amount",
        Description = "Total revenue statistics for log type and total liter, total amount by station id"
    )]
    public static async Task<IResult> GetLogByLogTypeAmountLiter([FromRoute] int id, [FromRoute] int logType, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByLogAmountLitersAsync(
            new GetLogPriceTotalResponse { StationId = id,  LogType=logType, From=From, To=To, FromTotal=FromTotal, ToTotal=ToTotal}, page, pageSize);
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
        Summary = "Report log get period, price, total liter",
        Description = "Total revenue statistics for the period, price, total liter by station id"
    )]
    public static async Task<IResult> GetLogByPeriodPriceLiters([FromRoute] int id, [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodPriceLitersAsync(new GetPeriodPriceTotalResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, From=From, To=To, FromTotal=FromTotal, ToTotal=ToTotal }, page, pageSize);
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
        Summary = "Report log get period, price, total amount",
        Description = "Total revenue statistics for the period, price, total amount by station id"
    )]
    public static async Task<IResult> GetLogByPeriodPriceAmount([FromRoute] int id, [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodPriceAmountAsync(new GetPeriodPriceTotalResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, From=From, To=To, FromTotal=FromTotal, ToTotal=ToTotal }, page, pageSize);
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
        Summary = "Report log get period, total amount, total liter",
        Description = "Total revenue statistics for the period, total amount, total liter by station id"
    )]
    public static async Task<IResult> GetLogByPeriodAmountLiters([FromRoute] int id, [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPeriodAmountLitersAsync(new GetPeriodPriceTotalResponse { StationId = id, ToDate = ToDate, FromDate = FromDate, From=From, To=To, FromTotal=FromTotal, ToTotal=ToTotal }, page, pageSize);
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
        Summary = "Report log get price, total amount, total liter",
        Description = "Total revenue statistics for the price, total amount, total liter by station id"
    )]
    public static async Task<IResult> GetLogByPriceAmountLiters([FromRoute] int id, [FromRoute] int FromAmount, [FromRoute] int ToAmount, [FromRoute] int From, [FromRoute] int To,
       [FromRoute] int FromTotal, [FromRoute] int ToTotal, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetLogByPriceAmountLitersAsync(new GetPriceAmountLitersResponse { StationId = id, From=From, To=To, FromTotal=FromTotal, ToTotal=ToTotal, FromAmount=FromAmount, ToAmount=ToAmount }, page, pageSize);
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
    //==================================================================
    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get full condition: dispenser name, period, fuel, log type",
        Description = "Total revenue statistics for the period, dispenser name, fuel name and log type by station id"
    )]
    public static async Task<IResult> GetLogByFullConditions([FromRoute] int id,  [FromRoute] DateTime FromDate, [FromRoute] DateTime ToDate, [FromRoute] int Name,
      [FromRoute] string FuelName, int LogType, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
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