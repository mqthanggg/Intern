using System.Text;

public static class ReportController
{
    public static WebApplication MapReport(this WebApplication app)
    {
        app.MapGet("shift/type/{id}", GetSumRevenueByType);
        app.MapGet("shift/name/{id}", GetSumRevenueByName);
        app.MapGet("day/type/{id}", GetSumRevenueDayByType);
        app.MapGet("day/name/{id}", GetSumRevenueDayByName);
        app.MapGet("month/type/{id}", GetSumRevenueMonthByType);
        app.MapGet("month/name/{id}", GetSumRevenueMonthByName);
        app.MapGet("year/type/{id}", GetSumRevenueYearByType);
        app.MapGet("year/name/{id}", GetSumRevenueYearByName);
        app.MapGet("sumstation", GetSumStation);
        app.MapGet("sumrevenuestation", GetTotalRevenueStation);
        app.MapGet("sumrevenue", GetSumRevenue);
        app.MapGet("sumrevenuetype7day", GetTotalRevenueByType7day);
        app.MapGet("sumrevenuebyname", GetTotalRevenueFullByName);
        app.MapGet("sumrevenuebytype", GetTotalRevenueFullByType);
        app.MapGet("sumrevenueday", GetTotalRevenueDay);
        app.MapGet("station/sumrevemue/{id}", GetSumRevenueByStation);
        app.MapGet("station/sumrevenueday/{id}", GetSumRevenueByStationDay);
        app.MapGet("station/sumrevenuemonth/{id}", GetSumRevenueByStationMonth);
        app.MapGet("station/sumrevenueyear/{id}", GetSumRevenueByStationYear);
        app.MapGet("station/getdate/{id}/{date}", GetSumRevenuegetDayByName);
        app.MapGet("station/getdate/{id}/{month}/{year}", GetSumRevenuegetMonthByName);
        app.MapGet("station/getdate/{id}/year/{year}", GetSumRevenuegetYearByName);
        app.MapGet("station/sumtyperevenueday/{id}/{date}", GetSumRevenueGetDayByType);
        app.MapGet("station/sumtyperevenuemonth/{id}/{month}/{year}", GetSumRevenuegetMonthByType);
        app.MapGet("station/sumtyperevenueyear/{id}/{year}", GetSumRevenuegetYearByType);
        app.MapGet("station/log/{id}", GetFullLogByStationId);
        app.MapGet("pagelog/station/{id}", GetPagedLogs);

        app.Map("ws/revenue", GetSumRevenueWS);
        app.Map("ws/station", GetSumStationWS);
        app.Map("ws/sumrevenue", GetTotalRevenueStationWS);
        app.Map("ws/type", GetTotalRevenueByType7dayWS);
        app.Map("ws/sumrenuename", GetTotalRevenueByNameWS);
        app.Map("ws/sumrenuetype", GetTotalRevenueByTypeWS);
        app.Map("ws/sumrevenueday", GetTotalRevenueDayWS);
        app.Map("ws/station/{id}", GetSumRevenueByStationWS);
        app.Map("ws/shift/type/{id}", GetSumRevenueShiftByTypeWS);
        app.Map("ws/shift/name/{id}", GetSumRevenueShiftByNameWS);
        app.Map("ws/day/type/{id}", GetSumRevenueDayByTypeWS);
        app.Map("ws/day/name/{id}", GetSumRevenueDayByNameWS);
        app.Map("ws/month/type/{id}", GetSumRevenueMonthByTypeWS);
        app.Map("ws/month/name/{id}", GetSumRevenueMonthByNameWS);
        app.Map("ws/year/type/{id}", GetSumRevenueYearByTypeWS);
        app.Map("ws/year/name/{id}", GetSumRevenueYearByNameWS);
        app.Map("ws/station/revenueday/{id}", GetSumRevenueByStationDayWS);
        app.Map("ws/station/revenuemonth/{id}", GetSumRevenueByStationMonthWS);
        app.Map("ws/station/revenueyear/{id}", GetSumRevenueByStationYearWS);
        app.Map("ws/sumrenuename/getdate/{id}/{date}", GetSumRevenuegetDayByNameWS);
        app.Map("ws/sumrenuetype/getdate/{id}/{date}", GetSumRevenuegetDayByTypeWS);
        app.Map("ws/sumrenuename/getmonth/{id}/{month}/{year}", GetSumRevenuegetMonthByNameWS);
        app.Map("ws/sumrenuetype/getmonth/{id}/{month}/{year}", GetSumRevenuegetMonthByTypeWS);
        app.Map("ws/sumrenuename/getyear/{id}/{year}", GetSumRevenuegetYearByNameWS);
        app.Map("ws/sumrenuetype/getyear/{id}/{year}", GetSumRevenuegetyearByTypeWS);
        app.Map("ws/{device}/{id}", GetWS);
        app.Map("ws/log/station/{id}", GetLogByStationWS);
        app.Map("ws/fulllog/station/{id}", GetFullLogByStationWS);
        app.Map("ws/pagelog/station/{id}", GetPageLogByStationWS);
        return app;
    }

    //======================== HTTP =============================
    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve device page a list of logs by station ID.",
        Description = "Retrieve device page a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task<IResult> GetPagedLogs([FromRoute] int id, [FromServices] ILogRepository logRepository, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var (logs, totalCount) = await logRepository.GetPageLogByStationIdAsync(new Station { StationId = id }, page, pageSize);
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

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve full logs by station ID.",
        Description = "Retrieve a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task<IResult> GetFullLogByStationId([FromRoute] int id, [FromServices] ILogRepository logRepository)
    {
        var res = await logRepository.GetFullLogByStationIdAsync(new Station { StationId = id });
        return TypedResults.Ok(
            res
        );
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByNameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report name get year",
        Description = "Total revenue statistics by name get year"
    )]
    public static async Task<IResult> GetSumRevenuegetYearByName([FromRoute] int id, [FromRoute] int year, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByNamegetYearAsync(new GetYearRevenue { StationId = id, Year = year });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByNameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report logtype get year",
        Description = "Total revenue statistics by logtype get year"
    )]
    public static async Task<IResult> GetSumRevenuegetYearByType([FromRoute] int id, [FromRoute] int year, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByTypeGetYearAsync(new GetYearRevenue { StationId = id, Year = year });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByNameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report name get month",
        Description = "Total revenue statistics by name get month"
    )]
    public static async Task<IResult> GetSumRevenuegetMonthByName([FromRoute] int id, [FromRoute] int month, [FromRoute] int year, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByNamegetMonthAsync(new GetMonthRevenue { StationId = id, Month = month, Year = year });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report logtype get month",
        Description = "Total revenue statistics by logtype get month"
    )]
    public static async Task<IResult> GetSumRevenuegetMonthByType([FromRoute] int id, [FromRoute] int month, [FromRoute] int year, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByTypeGetMonthAsync(new GetMonthRevenue { StationId = id, Month = month, Year = year });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByNameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report name get day",
        Description = "Total revenue statistics for the day by name"
    )]
    public static async Task<IResult> GetSumRevenuegetDayByName([FromRoute] int id, [FromRoute] DateTime date, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByNamegetDayAsync(new GetDateRevenue { StationId = id, Time = date });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report logtype get day",
        Description = "Total revenue statistics for the current day by logtype get day"
    )]
    public static async Task<IResult> GetSumRevenueGetDayByType([FromRoute] int id, [FromRoute] DateTime date, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByTypeGetDayAsync(new GetDateRevenue { StationId = id, Time = date });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueStationByYearResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
       Summary = "Report revenue with year of each station",
       Description = "Report total of revenue in each station by year, such as liters, revenue, import, profit"
   )]
    public static async Task<IResult> GetSumRevenueByStationYear([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueStationYearAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueStationByMonthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report revenue with month of each station",
        Description = "Report total of revenue in each station by month, such as liters, revenue, import, profit"
    )]
    public static async Task<IResult> GetSumRevenueByStationMonth([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueStationMonthAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }


    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByDateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report revenue with day of each station",
        Description = "Report total of revenue in each station by day, such as liters, revenue, import, profit"
    )]
    public static async Task<IResult> GetSumRevenueByStationDay([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueStationDayAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumStationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report revenue of each station",
        Description = "Report total of revenue in each station, such as liters, revenue, import, profit"
    )]
    public static async Task<IResult> GetSumRevenueByStation([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByStationAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<SumRevenueByDateResponse>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report total of revenue by day",
        Description = "Report total of revenue by day"
    )]
    public static async Task<IResult> GetTotalRevenueDay(IRevenueRepository revenueRepository)
    {
        try
        {
            var res = await revenueRepository.GetTotalRevenueByDayAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<SumRevenueByNameResponse>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report total of revenue by name",
        Description = "Report total of revenue by name"
    )]
    public static async Task<IResult> GetTotalRevenueFullByName(IRevenueRepository revenueRepository)
    {
        try
        {
            var res = await revenueRepository.GetTotalRevenueFullBynameAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<SumRevenueByTypeResponse>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report total of revenue by type",
        Description = "Report total of revenue by type"
    )]
    public static async Task<IResult> GetTotalRevenueFullByType(IRevenueRepository revenueRepository)
    {
        try
        {
            var res = await revenueRepository.GetTotalRevenueFullByTypeAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<SumRevenueByTypeResponse>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report total of revenue by type",
        Description = "Report total of revenue in 7 days by type, such as Ban le, Cong no, Khuyen mai, Tra truoc"
    )]
    public static async Task<IResult> GetTotalRevenueByType7day(IRevenueRepository revenueRepository)
    {
        try
        {
            var res = await revenueRepository.GetTotalRevenueByType7dayAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<RevenueResponse>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report total of revenue",
        Description = "Report total of revenue, such as liters, revenue, import, profit"
    )]
    public static async Task<IResult> GetSumRevenue(IRevenueRepository revenueRepository)
    {
        try
        {
            var res = await revenueRepository.GetTotalRevenueAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<SumStationResponse>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report total of each station",
        Description = "Total revenue for each station, such as liters, revenue, import, profit"
    )]
    public static async Task<IResult> GetTotalRevenueStation(IRevenueRepository revenueRepository)
    {
        try
        {
            var res = await revenueRepository.GetTotalRevenueStationtAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report total stations",
        Description = "Total the number of stations for the current in table station"
    )]
    public static async Task<IResult> GetSumStation(IStationRepository stationRepository)
    {
        try
        {
            var res = await stationRepository.GetSumStationResponseAsync();
            return TypedResults.Ok("Total stations: " + res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report type for shift",
        Description = "Total revenue statistics for the current shift by type"
    )]
    public static async Task<IResult> GetSumRevenueByType([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByTypeShiftAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByNameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report name for shift",
        Description = "Total revenue statistics for the current shift by name"
    )]
    public static async Task<IResult> GetSumRevenueByName([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByNameShiftAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report type for day",
        Description = "Total revenue statistics for the current day by type"
    )]
    public static async Task<IResult> GetSumRevenueDayByType([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByTypeDayAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByNameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report name for day",
        Description = "Total revenue statistics for the current day by name"
    )]
    public static async Task<IResult> GetSumRevenueDayByName([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByNameDayAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report type for month",
        Description = "Total revenue statistics for the current month by type"
    )]
    public static async Task<IResult> GetSumRevenueMonthByType([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByTypeMonthAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByNameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report name for month",
        Description = "Total revenue statistics for the current month by name"
    )]
    public static async Task<IResult> GetSumRevenueMonthByName([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByNameMonthAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
       Summary = "Report type for year",
       Description = "Total revenue statistics for the current year by type"
   )]
    public static async Task<IResult> GetSumRevenueYearByType([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByTypeYearAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(SumRevenueByNameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report name for year",
        Description = "Total revenue statistics for the current year by name"
    )]
    public static async Task<IResult> GetSumRevenueYearByName([FromRoute] int id, [FromServices] IRevenueRepository revenueRepository)
    {
        var result = await revenueRepository.GetTotalRevenueByNameYearAsync(new GetIdRevenue { StationId = id });

        if (result == null)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok(result);
    }
    // ==============================================================
    // ============================= WS =============================
    [SwaggerOperation(
       Summary = "Obtain web socket for devices",
       Description = "Return a web socket for the device with corresponding id for real-time data streaming."
   )]
    public static async Task GetWS(
        HttpContext context, 
        [FromRoute] string device, 
        [FromRoute] int id, 
        [FromQuery] string token, 
        [FromServices] IWebSocketHubService hubService, 
        [FromServices] ILogger<object> logger, 
        [FromServices] IJWTService jWTService)
    {
        if (context.WebSockets.IsWebSocketRequest && jWTService.Verify(token))
        {
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[1024 * 4];
            await hubService.StartAsync();
            await hubService.JoinDevice(device, id);
            var connection = hubService.GetHubConnection();
            var subscription = connection.On<byte[]>("SendMessage", async (message) =>
            {
                try
                {
                    await socket.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed, why: {e.Message}");
                }
            });
            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    await socket.ReceiveAsync(buffer, CancellationToken.None);
                }
            }
            catch (WebSocketException e)
            {
                Console.WriteLine($"Websocket closed, reason: {e.Message}");
            }
            subscription.Dispose();
            await connection.InvokeAsync("LeftDevice", device, id);
            await hubService.StopAsync();
        }
    }

     [SwaggerOperation(
        Summary = "Obtain web socket for logs by station ID.",
        Description = "Return a web socket for a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task GetPageLogByStationWS(
        HttpContext context, 
        [FromRoute] int id, 
        [FromServices] ILogRepository logRepository,
        [FromServices] ILogger<object> logger, 
        [FromQuery]int page = 1, 
        [FromQuery] int pageSize = 50)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await logRepository.GetPageLogByStationIdAsync(new Station { StationId = id }, page, pageSize);
                logger.LogDebug($"Result: {JsonSerializer.Serialize(result)}");
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for logs by station ID.",
        Description = "Return a web socket for a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task GetFullLogByStationWS(HttpContext context, [FromRoute] int id, [FromServices] ILogRepository logRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await logRepository.GetFullLogByStationIdAsync(new Station { StationId = id });
                logger.LogDebug($"Result: {JsonSerializer.Serialize(result)}");
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for limit logs by station ID.",
        Description = "Return a web socket for a list of limit logs that are related to the dispensers belong to the given station."
    )]
    public static async Task GetLogByStationWS(HttpContext context, [FromRoute] int id, [FromServices] ILogRepository logRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await logRepository.GetLogByStationIdAsync(new Station { StationId = id });
                logger.LogDebug($"Result: {JsonSerializer.Serialize(result)}");
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for logs by station ID.",
        Description = "Return a web socket for a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task GetByStationWS(HttpContext context, [FromRoute] int id, [FromServices] ILogRepository tanlRepository, [FromServices] ILogger<object> logger)
    {
        logger.LogInformation($"WebSocket text");
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await tanlRepository.GetLogByStationIdAsync(new Station { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }


    [SwaggerOperation(
         Summary = "Obtain web socket for total name get year",
         Description = "Return a web socket for total revenue statistics get year by name"
    )]
    public static async Task GetSumRevenuegetYearByNameWS(HttpContext context, [FromRoute] int id,
         [FromRoute] int year, [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByNamegetYearAsync(new GetYearRevenue { StationId = id, Year = year });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total logtype get day",
        Description = "Return a web socket for total revenue statistics get logtype by name"
     )]
    public static async Task GetSumRevenuegetyearByTypeWS(HttpContext context, [FromRoute] int id, [FromRoute] int year,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByTypeGetYearAsync(new GetYearRevenue { StationId = id, Year = year });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total name get month",
        Description = "Return a web socket for total revenue statistics get month by name"
    )]
    public static async Task GetSumRevenuegetMonthByNameWS(HttpContext context, [FromRoute] int id, [FromRoute] int month,
         [FromRoute] int year, [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByNamegetMonthAsync(new GetMonthRevenue { StationId = id, Month = month, Year = year });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total logtype get month",
        Description = "Return a web socket for total revenue statistics get logtype by month"
    )]
    public static async Task GetSumRevenuegetMonthByTypeWS(HttpContext context, [FromRoute] int id, [FromRoute] int month, [FromRoute] int year,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByTypeGetMonthAsync(new GetMonthRevenue { StationId = id, Month = month, Year = year });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
         Summary = "Obtain web socket for total name get day",
         Description = "Return a web socket for total revenue statistics get day by name"
     )]
    public static async Task GetSumRevenuegetDayByNameWS(HttpContext context, [FromRoute] int id, [FromRoute] DateTime date,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByNamegetDayAsync(new GetDateRevenue { StationId = id, Time = date });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                try
                {
                    var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000)); // timeout 3s

                    if (completedTask == receiveTask)
                    {
                        if (receiveTask.IsCompletedSuccessfully && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                        {
                            logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                            break;
                        }
                    }
                    else
                    {
                        logger.LogWarning($"Timeout while waiting for WebSocket message for stationId: {id}");
                        // Xử lý nếu cần
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"WebSocket error at stationId: {id}");
                    break;
                }

            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
         Summary = "Obtain web socket for total logtype get day",
         Description = "Return a web socket for total revenue statistics get logtype by name"
     )]
    public static async Task GetSumRevenuegetDayByTypeWS(HttpContext context, [FromRoute] int id, [FromRoute] DateTime date,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByTypeGetDayAsync(new GetDateRevenue { StationId = id, Time = date });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for report total of each station",
        Description = "Return a web socket for total revenue for each station, such as liters, revenue, import, profit"
    )]
    public static async Task GetSumRevenueByStationWS(HttpContext context, [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByStationAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total station by day",
        Description = "Return a web socket for total station of each stations by day in the system."
    )]
    public static async Task GetSumRevenueByStationDayWS(HttpContext context, [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueStationDayAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total station by month",
        Description = "Return a web socket for total station of each stations by month in the system."
    )]
    public static async Task GetSumRevenueByStationMonthWS(HttpContext context, [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueStationMonthAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
      Summary = "Obtain web socket for total station by year",
      Description = "Return a web socket for total station of each stations by year in the system."
    )]
    public static async Task GetSumRevenueByStationYearWS(HttpContext context, [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueStationYearAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
       Summary = "Obtain web socket for report revenue with year of each station",
       Description = "Return a web socket for report total of revenue in each station by year, such as liters, revenue, import, profit"
   )]
    public static async Task GetSumRevenueWS(HttpContext context, [FromServices] IRevenueRepository revenueRepository)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueAsync();
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            TypedResults.NotFound(ex);
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total stations",
        Description = "Return a web socket for total the number of stations for the current in table station"
    )]
    public static async Task GetTotalRevenueStationWS(HttpContext context, [FromServices] IRevenueRepository revenueRepository)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueStationtAsync();
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            TypedResults.NotFound(ex);
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total stations",
        Description = "Return a web socket for total the number of stations for the current in table station"
    )]
    public static async Task GetSumStationWS(HttpContext context, [FromServices] IStationRepository stationRepository)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await stationRepository.GetSumStationResponseAsync();
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            TypedResults.NotFound(ex);
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total of revenue by type",
        Description = "Return a web socket for total of revenue in 7 days by type, such as Ban le, Cong no, Khuyen mai, Tra truoc"
    )]
    public static async Task GetTotalRevenueByType7dayWS(HttpContext context, [FromServices] IRevenueRepository revenueRepository)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByType7dayAsync();
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            TypedResults.NotFound(ex);
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total of revenue by name",
        Description = "Return a web socket for total of revenue by name"
    )]
    public static async Task GetTotalRevenueByNameWS(HttpContext context, [FromServices] IRevenueRepository revenueRepository)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueFullBynameAsync();
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            TypedResults.NotFound(ex);
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total of revenue by type",
        Description = "Return a web socket for total of revenue by type"
    )]
    public static async Task GetTotalRevenueByTypeWS(HttpContext context, [FromServices] IRevenueRepository revenueRepository)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueFullByTypeAsync();
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            TypedResults.NotFound(ex);
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total of revenue by day",
        Description = "Return a web socket for total of revenue by day"
    )]
    public static async Task GetTotalRevenueDayWS(HttpContext context, [FromServices] IRevenueRepository revenueRepository)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByDayAsync();
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            TypedResults.NotFound(ex);
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total type for shift",
        Description = "Return a web socket for total revenue statistics for the current shift by type"
    )]
    public static async Task GetSumRevenueShiftByTypeWS(HttpContext context,
        [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository,
        [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByTypeShiftAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total name for shift",
        Description = "Return a web socket for total revenue statistics for the current shift by name"
    )]
    public static async Task GetSumRevenueShiftByNameWS(HttpContext context, [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository, [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByNameShiftAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
         Summary = "Obtain web socket for total name for day",
         Description = "Return a web socket for total revenue statistics for the current day by name"
     )]
    public static async Task GetSumRevenueDayByNameWS(HttpContext context,
        [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository,
        [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByNameDayAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
       Summary = "Obtain web socket for total type for day",
       Description = "Return a web socket for total revenue statistics for the current day by type"
   )]
    public static async Task GetSumRevenueDayByTypeWS(HttpContext context,
       [FromRoute] int id,
       [FromServices] IRevenueRepository revenueRepository,
       [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByTypeDayAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total name for month",
        Description = "Return a web socket for total revenue statistics for the current month by name"
    )]
    public static async Task GetSumRevenueMonthByNameWS(HttpContext context,
       [FromRoute] int id,
       [FromServices] IRevenueRepository revenueRepository,
       [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByNameMonthAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
       Summary = "Obtain web socket for total type for month",
       Description = "Return a web socket for total revenue statistics for the current month by type"
   )]
    public static async Task GetSumRevenueMonthByTypeWS(HttpContext context,
        [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository,
        [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByTypeMonthAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
        Summary = "Obtain web socket for total name for year",
        Description = "Return a web socket for total revenue statistics for the current year by name"
    )]
    public static async Task GetSumRevenueYearByNameWS(HttpContext context,
       [FromRoute] int id,
       [FromServices] IRevenueRepository revenueRepository,
       [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByNameYearAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000));  // delay 3s
                if (completedTask == receiveTask && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }

    [SwaggerOperation(
           Summary = "Obtain web socket for total type for year",
           Description = "Return a web socket for total revenue statistics for the current year by type"
       )]
    public static async Task GetSumRevenueYearByTypeWS(HttpContext context,
        [FromRoute] int id,
        [FromServices] IRevenueRepository revenueRepository,
        [FromServices] ILogger<object> logger)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = 400;
            return;
        }
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        logger.LogInformation($"WebSocket connection opened for stationId: {id}");
        var receiveBuffer = new byte[1024 * 4];
        string? previousJson = null;
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await revenueRepository.GetTotalRevenueByTypeMonthAsync(new GetIdRevenue { StationId = id });
                var currentJson = JsonSerializer.Serialize(result);
                if (currentJson != previousJson)
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(currentJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    previousJson = currentJson;
                }
                if (socket.State != WebSocketState.Open)
                    break;
                try
                {
                    var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    var completedTask = await Task.WhenAny(receiveTask, Task.Delay(3000)); // timeout 3s

                    if (completedTask == receiveTask)
                    {
                        if (receiveTask.IsCompletedSuccessfully && receiveTask.Result.MessageType == WebSocketMessageType.Close)
                        {
                            logger.LogInformation($"WebSocket connection closed by client for stationId: {id}");
                            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                            break;
                        }
                    }
                    else
                    {
                        logger.LogWarning($"Timeout while waiting for WebSocket message for stationId: {id}");
                        // Xử lý nếu cần
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"WebSocket error at stationId: {id}");
                    break;
                }

            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"WebSocket error for stationId: {id}");
        }
    }
}