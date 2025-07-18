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
        app.Map("ws/station/sumrevenueday/{id}", GetSumRevenueByStationDayWS);
        app.Map("ws/{device}/{id}", GetWS);
        return app;
    }

    //===========================================================
    //======================== HTTP =============================
     [Authorize]
    [HttpGet("sumrevenuebystatioyear/{id}")]
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
    [HttpGet("sumrevenuebystationmonth/{id}")]
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
    [HttpGet("sumrevenuebystationday/{id}")]
    [ProducesResponseType(typeof(SumRevenueStationByDateResponse), StatusCodes.Status200OK)]
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
    [HttpGet("sumrevenuebystation/{id}")]
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
    [HttpGet("sumrevenueday")]
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
    [HttpGet("sumrevenuefullname")]
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
    [HttpGet("sumrevenuefulltype")]
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
    [HttpGet("sumrevenuetype7days")]
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
    [HttpGet("sumrevenue")]
    [ProducesResponseType(typeof(List<SumRevenueResponse>), 200)]
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
    [HttpGet("sumrevenuestation")]
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
    [HttpGet("sumstation")]
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
    [HttpGet("sumrevenuetypeshift/{id}")]
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
    [HttpGet("sumrevenuenameshift/{id}")]
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
    [HttpGet("sumrevenuetypeday/{id}")]
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
    [HttpGet("sumrevenuenameday/{id}")]
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
    [HttpGet("sumrevenuetypemonth/{id}")]
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
    [HttpGet("sumrevenuenamemonth/{id}")]
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
    [HttpGet("sumrevenuetypeyear/{id}")]
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
    [HttpGet("sumrevenuenamebyyear/{id}")]
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
    public static async Task GetWS(HttpContext context, [FromRoute] string device, [FromRoute] int id, [FromQuery] string token, IMqttService mqttService, ILogger<object> logger, IJWTService jWTService)
    {
        if (context.WebSockets.IsWebSocketRequest && jWTService.Verify(token))
        {
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            mqttService.AddSocket($"{device}:{id}", socket);
            var buffer = new byte[1024 * 4];
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
            finally
            {
                mqttService.RemoveSocket($"{device}:{id}", socket);
            }
        }
    }
    public static async Task GetSumRevenueByStationWS(HttpContext context,
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

    public static async Task GetSumRevenueShiftByNameWS(HttpContext context,
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

}