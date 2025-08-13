public static class PublicController
{
    public static WebApplication MapPublicController(this WebApplication app)
    {
        app.MapGet("log/station/{id}", GetLogByStationId);
        app.MapGet("dispenser/station/{id}", GetDispenserByStationId);
        app.MapGet("tank/station/{id}", GetTankByStationId);
        app.MapGet(".well-known/jwks.json", GetJWKs);
        app.MapGet("stations", GetStations);
        app.MapGet("station/{id}", GetStationByStationId);
        app.MapGet("staff/{id}", GetStaffByStaffId);
        app.MapGet("staffs", GetStaffs);
        app.MapGet("shifts", GetShift);
        app.MapGet("shift/{id}", GetShiftByShiftId);
        app.MapGet("assignments", GetAssignment);
        app.MapGet("fuels", GetFuels);
        app.MapPost("assignments/station", GetAssignmentByStationIdAndDate);

        app.MapPost("login", Login);
        app.MapPost("register", RegisterAccount);
        app.MapPost("refresh", RefreshJWT);
        app.MapPost("logout", Logout);

        app.MapDelete("station/{id}", DeleteStationFromId);
        app.MapPut("station/{id}", UpdateStationFromId);
        app.MapPut("log/update", UpdateLogTime);
        app.MapPost("accounts/{page}", GetAccounts);
        app.MapPut("account/{id}", ChangeAccountPassword);
        app.MapDelete("account/{id}", RemoveAccount);
        app.MapPost("account", SignUpAccount);
        app.MapPost("get/fulltime/filter/{id}", GetFullLogByFilterConditions);
        app.MapPost("get/full/filter/{id}", GetLogByFilterConditions);
        return app;
    }

    private static string RoleMapping(int role)
    {
        return role switch
        {
            1 => "user",
            2 => "administrator",
            _ => "",
        };
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(typeof(List<FuelResponse>), 200)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Get all fuels",
        Description = "Obtain a list of all fuels"
    )]
    public static async Task<IResult> GetFuels(
        [FromServices] IFuelRepository FuelRepository
    )
    {
        try
        {
            var res = await FuelRepository.GetFullFuel();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }


    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(409)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Register account",
        Description = "Register an account from the given username and password"
    )]
    private static async Task<IResult> RegisterAccount(
        [FromBody] RegisterRequest body,
        [FromServices] IHasher hasher,
        [FromServices] IUserRepository userRepository
    )
    {
        if (
            string.IsNullOrWhiteSpace(body.Username) ||
            string.IsNullOrWhiteSpace(body.Password)
        )
            return TypedResults.BadRequest(new ErrorResponse { Why = "Missing username or password" });
        var (HashedPassword, PasswordPadding) = hasher.Hash(new { }, body.Password);
        var (HashedRefreshToken, RefreshTokenPadding) = hasher.Hash(new { }, Guid.NewGuid().ToString());
        var user = new User
        {
            Username = body.Username,
            Password = HashedPassword,
            Padding = PasswordPadding,
            RefreshToken = HashedRefreshToken,
            TokenPadding = RefreshTokenPadding,
            TokenExpiredTime = DateTime.UtcNow.AddDays(7),
            CreatedBy = body.Username,
            LastModifiedBy = body.Username
        };

        try
        {
            int rows = await userRepository.InsertAsync(user);
            if (rows == 0)
                return TypedResults.BadRequest(new ErrorResponse { Why = "Could not insert user" });

            return Results.Created();
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            return Results.Conflict();
        }
        catch (Exception)
        {
            return TypedResults.NotFound();
        }
    }

    // [Authorize]
    // [Permission("user")]
    // [RequireAntiforgeryToken]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Update log's time.",
        Description = "Update log's time."
    )]
    public static async Task<IResult> UpdateLogTime(
        [FromBody] LogResponse entity,
        [FromServices] ILogRepository logRepository
    )
    {
        var result = await logRepository.UpdateLogTimeAsync(entity);
        if (result == 0)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok();
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(typeof(StaffResponse), 200)]
    [ProducesResponseType(404)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Get staff by ID",
        Description = "Returns staff information by ID"
    )]
    public static async Task<IResult> GetStaffByStaffId(
        [FromRoute] int id,
        [FromServices] IStaffRepository StaffRepository
    )
    {
        var res = await StaffRepository.GetStaffByIdAsync(new Staff { StaffId = id });
        if (res == null)
            return Results.NotFound();
        return TypedResults.Ok(res);
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(typeof(List<StaffResponse>), 200)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Get all staffs",
        Description = "Obtain a list of all staffs"
    )]
    public static async Task<IResult> GetStaffs(
        [FromServices] IStaffRepository StaffRepository
    )
    {
        try
        {
            var res = await StaffRepository.GetAllStaffResponseAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(List<ShiftResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain shifts.",
        Description = "Obtain a list of all shifts."
    )]
    public static async Task<IResult> GetShift(
        [FromServices] IShiftRepository shiftRepository
    )
    {
        try
        {
            var res = await shiftRepository.GetAllShiftResponseAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(ShiftResponse), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain a shift by id",
        Description = "Obtain a shift by id"
    )]
    public static async Task<IResult> GetShiftByShiftId(
        [FromRoute] int id,
        [FromServices] IShiftRepository ShiftRepository
    )
    {
        var res = await ShiftRepository.GetShiftById(new Shift { ShiftId = id });
        if (res == null)
            return Results.NotFound();
        return TypedResults.Ok(res);
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(List<AssignmentResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain assignment.",
        Description = "Obtain a list of all Assignments."
    )]
    public static async Task<IResult> GetAssignment(
        [FromServices] IAssignmentRepository AssignmentRepository
    )
    {
        try
        {
            var res = await AssignmentRepository.GetAllAssignmentResponseAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
    }

    // [Authorize]
    // [Permission("user")]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve logs by station ID.",
        Description = "Retrieve a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task<IResult> GetLogByStationId(
        [FromRoute] int id,
        [FromServices] ILogRepository logRepository
    )
    {
        var res = await logRepository.GetLogByStationIdAsync(new Station { StationId = id });
        return TypedResults.Ok(
            res
        );
    }

    // [Authorize]
    // [Permission("user")]
    [ProducesResponseType(typeof(List<DispenserResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve dispensers by station ID.",
        Description = "Retrieve a list of dispensers that belong to the given station."
    )]
    public static async Task<IResult> GetDispenserByStationId(
        [FromRoute] int id,
        [FromServices] IDispenserRepository dispenserRepository
    )
    {
        var res = await dispenserRepository.GetDispensersByStationIdAsync(new Station { StationId = id });
        return TypedResults.Ok(
            res
        );
    }

    // [Authorize]
    // [Permission("user")]
    [ProducesResponseType(typeof(List<TankResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve tanks by station ID.",
        Description = "Retrieve a list of tanks that are belong to the given station."
    )]
    public static async Task<IResult> GetTankByStationId(
        [FromRoute] int id,
        [FromServices] ITankRepository tankRepository, [FromServices] IJWTService jWTService
    )
    {
        var res = await tankRepository.GetTanksByStationIdAsync(new Station { StationId = id });
        return TypedResults.Ok(
            res
        );
    }

    [ProducesResponseType(typeof(List<JsonWebKey>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve JWKs",
        Description = "Retrieve a list of JSON web key."
    )]
    public static IResult GetJWKs(
        [FromServices] IJWKsService jWKs
    )
    {
        return TypedResults.Ok(
            jWKs.GetJWKs()
      );
    }

    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Login",
        Description = "Resolve login request from the client."
    )]
    public static async Task<IResult> Login(
        HttpContext context,
        [FromBody] LoginRequest body,
        [FromServices] IUserRepository userRepository,
        [FromServices] IHasher hasher,
        [FromServices] IJWTService jwt,
        [FromServices] IAntiforgery antiforgery)
    {
        try
        {
            var user = await userRepository.GetUserByUsernameAsync(new User { Username = body.Username });
            if (hasher.Verify(user, body.Password + user.Padding, user.Password!))
            {
                string randomRefreshToken = Guid.NewGuid().ToString();
                int res;
                while (true)
                {
                    (string hashed, string padding) = hasher.Hash(new { }, randomRefreshToken);
                    try
                    {
                        res = await userRepository.UpdateAsync(new User
                        {
                            UserId = user.UserId,
                            RefreshToken = hashed,
                            TokenPadding = padding,
                            TokenExpiredTime = DateTime.UtcNow.AddDays(7),
                            LastModifiedBy = body.Username,
                        });
                    }
                    catch (PostgresException e) when (e.SqlState == "23505")
                    {
                        Regex regex = new Regex(@"user_(\w+)_key");
                        Match match = regex.Match(e.ConstraintName ?? "");
                        string column = match.Groups[1].Value;
                        if (column == "token_padding")
                            continue;
                        return TypedResults.InternalServerError(new ErrorResponse { Why = column });
                    }
                    catch (PostgresException e)
                    {
                        return TypedResults.InternalServerError(new ErrorResponse { Why = e.Message });
                    }
                    break;
                }
                if (res > 0)
                {
                    var XSRF = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append(
                        "XSRF-COOKIE",
                        XSRF.RequestToken ?? "",
                        new CookieOptions
                        {
                            Path = "/",
                            HttpOnly = false,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddDays(7),
                            MaxAge = TimeSpan.FromDays(7)
                        }
                    );
                    return Results.Ok(new
                    LoginResponse
                    {
                        Token = jwt.GenerateAccessToken(user.UserId!.Value, user.Username!, RoleMapping(user.Role!.Value)),
                        RefreshToken = randomRefreshToken,
                        Role = RoleMapping(user.Role!.Value)
                    });
                }
            }
            return TypedResults.Unauthorized();
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Failed, why: {e}");
            return TypedResults.NotFound();
        }
    }
    // [Authorize]
    // [Permission("user")]
    // [RequireAntiforgeryToken]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Delete station",
        Description = "Delete a station from a given id."
    )]
    public static async Task<IResult> DeleteStationFromId(
        [FromRoute] int id,
        [FromServices] IStationRepository stationRepository
    )
    {
        int res;
        try
        {
            res = await stationRepository.DeleteAsync(new Station { StationId = id });
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
        if (res == 1)
            return TypedResults.Ok();
        return TypedResults.NotFound();
    }

    // [Authorize]
    // [Permission("user")]
    // [RequireAntiforgeryToken]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Update station",
        Description = "Update name, address of a station from a given id."
    )]
    public static async Task<IResult> UpdateStationFromId(
        [FromRoute] int id,
        [FromBody] StationUpdateRequest body,
        [FromServices] IStationRepository stationRepository,
        [FromServices] IJWTService jWTService)
    {
        int res;
        try
        {
            res = await stationRepository.UpdateAsync(new Station { StationId = id, Name = body.Name, Address = body.Address });
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
        if (res == 1)
            return TypedResults.Ok();
        return TypedResults.NotFound();
    }

    // [Authorize]
    // [Permission("user")]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(List<StationResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain stations.",
        Description = "Obtain a list of all stations."
    )]
    public static async Task<IResult> GetStations(
        [FromServices] IStationRepository stationRepository
    )
    {
        try
        {
            var res = await stationRepository.GetAllStationResponseAsync();
            return TypedResults.Ok(res);
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
            return TypedResults.InternalServerError();
        }
    }


    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(ShiftResponse), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain a station by id",
        Description = "Obtain a station by id"
    )]
    public static async Task<IResult> GetStationByStationId(
        [FromRoute] int id,
        [FromServices] IStationRepository StationRepository
    )
    {
        var res = await StationRepository.GetStationById(new Station { StationId = id });
        if (res == null)
            return Results.NotFound();
        return TypedResults.Ok(res);
    }

    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Refresh access token.",
        Description = "Return a new access token if the refresh token is valid and not expired to the user."
    )]
    public static async Task<IResult> RefreshJWT(
        [FromServices] ILogger<object> logger,
        [FromBody] TokenRequest tokenRequest,
        [FromServices] IUserRepository userRepository,
        [FromServices] IJWTService jWTService,
        [FromServices] IHasher hasher)
    {
        try
        {
            var claims = new JwtSecurityTokenHandler().
            ReadJwtToken(
                tokenRequest.Token
            ).Claims;
            int userId = Convert.ToInt32(claims.First(e => e.Type == ClaimTypes.Sid).Value);
            string username = claims.First(e => e.Type == ClaimTypes.Name).Value;
            var user = await userRepository.GetUserTokenAsync(new User
            {
                UserId = userId,
                Username = username
            });
            if (
                user.RefreshToken == null ||
                user.TokenPadding == null ||
                user.TokenExpiredTime == null ||
                DateTime.UtcNow.CompareTo(user.TokenExpiredTime) > 0 ||
                !hasher.Verify(user, tokenRequest.RefreshToken + user.TokenPadding, user.RefreshToken)
            )
                return TypedResults.Unauthorized();

            return TypedResults.Ok(
                new TokenResponse
                {
                    Token = jWTService.GenerateAccessToken(
                        userId,
                        username,
                        claims.First(e => e.Type == ClaimTypes.Role).Value
                    )
                }
            );
        }
        catch (Exception ex)
        {
            if (ex is PostgresException || ex is SecurityTokenMalformedException)
            {
                return TypedResults.Unauthorized();
            }
            return TypedResults.InternalServerError();
        }

    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Logout user",
        Description = "Destroy refresh token to invalidate future requests if the user does not sign in again."
    )]
    public static async Task<IResult> Logout(
        HttpContext context,
        [FromBody] LogOutRequest logOutRequest,
        [FromServices] IUserRepository userRepository,
        [FromServices] IJWTService jWTService
    )
    {
        try
        {
            if (jWTService.Verify(logOutRequest.Token))
            {
                var claims = new JwtSecurityTokenHandler().
                ReadJwtToken(
                logOutRequest.Token
                ).Claims;
                var username = claims.First(e => e.Type == ClaimTypes.Name).Value;
                var userId = Convert.ToInt32(claims.First(e => e.Type == ClaimTypes.Sid).Value);
                var res = await userRepository.UpdateAsync(
                    new User
                    {
                        RefreshToken = null,
                        TokenPadding = null,
                        TokenExpiredTime = null,
                        LastModifiedBy = username,
                        UserId = userId
                    }
                );
                if (res > 0)
                {
                    context.Response.Cookies.Delete("XSRF-COOKIE");
                    return TypedResults.Ok();
                }
            }
            return TypedResults.Unauthorized();
        }
        catch (PostgresException)
        {
            return TypedResults.Unauthorized();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Why: {ex.Message}");
            return TypedResults.InternalServerError();
        }
    }

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(List<AssignmentResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain assignments by station id.",
        Description = "Obtain a list of all assignments by station id."
    )]
    public static async Task<IResult> GetAssignmentByStationIdAndDate(
        [FromServices] IAssignmentRepository AssignmentRepository,
        [FromBody] AssignmentRequest body
    )
    {
        try
        {
            var res = await AssignmentRepository.GetAllAssignmentResponseByStationIdAndDateAsync(new Assignment { StationId = body.StationId, WorkDate = body.WorkDate });
            return TypedResults.Ok(res);
        }
        catch (PostgresException e)
        {
            Console.WriteLine($"why: {e.Message}");
            return TypedResults.InternalServerError();
        }
    }

    //==============================================
    [SwaggerOperation(
        Summary = "Obtain web socket for report total of each station",
        Description = "Return a web socket for total revenue for each station, such as liters, revenue, import, profit"
    )]
    public static async Task GetStationByStationWS(HttpContext context, [FromRoute] int id,
        [FromServices] IStationRepository stationRepository, [FromServices] ILogger<object> logger)
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
                var result = await stationRepository.GetStationById(new Station { StationId = id });
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

    // [Authorize]
    // [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(List<UserResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain accounts list.",
        Description = "Obtain a list of all accounts."
    )]
    public static async Task<IResult> GetAccounts(
        [FromServices] IUserRepository userRepository,
        [FromRoute] int page,
        [FromBody] UserRequestFilter body
    )
    {
        try
        {
            var PaginationSetting = new PaginationSetting(
                15, page
            );
            var users = await userRepository.GetUserWithActiveAsync(
                new UserRequestFilterWithPagination
                {
                    Limit = PaginationSetting.Limit,
                    Offset = PaginationSetting.Offset,
                    Username = body.Username + "%",
                    Role = body.Role,
                    Active = body.Active
                }
            );

            var counts = await userRepository.GetUserCountWithFilterAsync(
                body
            );

            return TypedResults.Ok(
                new UserResponseWithPage
                {
                    Users = users,
                    PageNumber = Convert.ToInt32(Math.Ceiling(counts / 15.0))
                }
            );
        }
        catch (PostgresException ex)
        {
            Console.WriteLine($"Failed, why: {ex}");
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Change account password.",
        Description = "Change account with a given id password"
    )]
    public static async Task<IResult> ChangeAccountPassword(
        [FromServices] IUserRepository userRepository,
        [FromServices] IHasher hasher,
        [FromRoute] int id,
        [FromBody] UserChangePasswordRequest body
    )
    {
        try
        {
            if (body.NewPassword != body.ReTypePassword)
                return TypedResults.Unauthorized();
            var user = await userRepository.GetUserByIdAsync(
                new User
                {
                    UserId = id
                }
            );
            if (user == null)
                return TypedResults.NotFound("UserId not found");
            if (hasher.Verify(
                user,
                body.OldPassword + user.Padding,
                user.Password ?? ""
            ))
            {
                (string NewHashedPassword, string NewPadding) = hasher.Hash(new object { }, body.NewPassword);
                int affectedRows = await userRepository.UpdateUserPasswordAsync(
                    new User
                    {
                        UserId = id,
                        Password = NewHashedPassword,
                        Padding = NewPadding
                    }
                );
                if (affectedRows != 1)
                {
                    throw new Exception("Cannot update user's password");
                }
                else
                {
                    return TypedResults.Ok();
                }
            }
            return TypedResults.Unauthorized();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed, why: {ex}");
            return TypedResults.InternalServerError();
        }
    }

    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 404)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Remove an account.",
        Description = "Remove an account from the given id."
    )]
    public static async Task<IResult> RemoveAccount(
        [FromRoute] int id,
        [FromServices] IUserRepository userRepository,
        [FromServices] IHasher hasher
    )
    {
        try
        {
            int affectedRows = await userRepository.DeleteAsync(
                new User
                {
                    UserId = id
                }
            );
            if (affectedRows != 1)
            {
                return TypedResults.NotFound("UserId not found.");
            }
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed, why {ex}");
            return TypedResults.InternalServerError();
        }
    }
    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Signup an account.",
        Description = "Signup a new account from the given information."
    )]
    public static async Task<IResult> SignUpAccount(
        [FromServices] IUserRepository userRepository,
        [FromServices] IHasher hasher,
        [FromBody] UserSignUpRequest body
    )
    {
        try
        {
            if (body.Password != body.ReTypePassword)
                return TypedResults.Unauthorized();
            (string HashedPassword, string Padding) = hasher.Hash(new object { }, body.Password);
            int affectedRows = await userRepository.InsertAsync(
                new User
                {
                    Username = body.Username,
                    Password = HashedPassword,
                    Padding = Padding,
                    Role = body.Role
                }
            );
            if (affectedRows != 1)
            {
                throw new Exception("Cannot insert new user.");
            }
            return TypedResults.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed, why: {e}");
            return TypedResults.InternalServerError();
        }
    }

    // [Authorize]
    [ProducesResponseType(typeof(LogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Report log get filter log condition",
        Description = "Return filter all log conditions by station id"
    )]
    public static async Task<IResult> GetFullLogByFilterConditions([FromServices] ILogRepository logRepository,
        [FromBody] GetFullNullConditionFilterResponse body, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 20;
        var (logs, totalCount) = await logRepository.GetFullNullConditionAsync(body, page, pageSize);
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
        Summary = "Report log get filter log condition not time",
        Description = "Return filter all log conditions by station id"
    )]
    public static async Task<IResult> GetLogByFilterConditions([FromServices] ILogRepository logRepository,
        [FromBody] GetFullNullConditionFilterResponse body, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 20;
        var (logs, totalCount) = await logRepository.GetFullLogConditionAsync(body, page, pageSize);
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