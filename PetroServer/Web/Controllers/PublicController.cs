public static class PublicController
{
    public static WebApplication MapPublicController(this WebApplication app)
    {
        app.MapGet("log/station/{id}", GetLogByStationId);
        app.MapGet("dispenser/station/{id}", GetDispenserByStationId);
        app.MapGet("tank/station/{id}", GetTankByStationId);
        app.MapGet(".well-known/jwks.json", GetJWKs);
        app.MapGet("stations", GetStations);
        app.MapGet("staff/{id}", GetStaffByStaffId);
        app.MapGet("staffs", GetStaffs);
        app.MapGet("shifts", GetShift);
        app.MapGet("shift/{id}", GetShiftByShiftId);
        app.MapGet("assignments", GetAssignment);

        app.MapPost("login", Login);
        app.MapPost("register", RegisterAccount);
        app.MapPost("refresh", RefreshJWT);

        app.MapDelete("station/{id}", DeleteStationFromId);
        app.MapPut("station/{id}", UpdateStationFromId);
        app.MapPut("log/update", UpdateLogTime);
        return app;
    }

    private static string RoleMapping(int role){
        return role switch
        {
            1 => "user",
            2 => "administrator",
            _ => "",
        };
    }

    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    [ProducesResponseType(typeof(ErrorResponse),404)]
    [ProducesResponseType(409)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Register account",
        Description = "Register an account from the given username and password"
    )]
    private static async Task<IResult> RegisterAccount([FromBody] RegisterRequest body, IHasher hasher, IUserRepository userRepository)
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

    [Authorize]
    [Permission("user")]
    [RequireAntiforgeryToken]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Update log's time.",
        Description = "Update log's time."
    )]
    public static async Task<IResult> UpdateLogTime([FromBody] LogResponse entity, [FromServices] ILogRepository logRepository)
    {
        var result = await logRepository.UpdateLogTimeAsync(entity);
        if (result == 0)
        {
            return Results.NotFound();
        }
        return TypedResults.Ok();
    }



    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(typeof(StaffResponse), 200)]
    [ProducesResponseType(404)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Get staff by ID",
        Description = "Returns staff information by ID"
    )]
    public static async Task<IResult> GetStaffByStaffId([FromRoute] int id, IStaffRepository StaffRepository)
    {
        var res = await StaffRepository.GetStaffByIdAsync(new Staff { StaffId = id });
        if (res == null)
            return Results.NotFound();
        return TypedResults.Ok(res);
    }

    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(typeof(List<StaffResponse>), 200)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Get all staffs",
        Description = "Obtain a list of all staffs"
    )]
    public static async Task<IResult> GetStaffs(IStaffRepository StaffRepository)
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

    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(List<ShiftResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain shifts.",
        Description = "Obtain a list of all shifts."
    )]
    public static async Task<IResult> GetShift(IShiftRepository shiftRepository)
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

    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(List<ShiftResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain a shift by id",
        Description = "Obtain a shift by id"
    )]
    public static async Task<IResult> GetShiftByShiftId([FromRoute] int id, IShiftRepository ShiftRepository)
    {
        var res = await ShiftRepository.GetShiftById(new Shift { ShiftId = id });
        if (res == null)
            return Results.NotFound();
        return TypedResults.Ok(res);
    }

    [Authorize]
    [Permission("administrator")]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(List<AssignmentResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain assignment.",
        Description = "Obtain a list of all Assignments."
    )]
    public static async Task<IResult> GetAssignment(IAssignmentRepository AssignmentRepository)
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

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<LogResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve logs by station ID.",
        Description = "Retrieve a list of logs that are related to the dispensers belong to the given station."
    )]
    public static async Task<IResult> GetLogByStationId([FromRoute] int id, ILogRepository logRepository)
    {
        var res = await logRepository.GetLogByStationIdAsync(new Station { StationId = id });
        return TypedResults.Ok(
            res
        );
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<DispenserResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve dispensers by station ID.",
        Description = "Retrieve a list of dispensers that belong to the given station."
    )]
    public static async Task<IResult> GetDispenserByStationId([FromRoute] int id, IDispenserRepository dispenserRepository)
    {
        var res = await dispenserRepository.GetDispensersByStationIdAsync(new Station { StationId = id });
        return TypedResults.Ok(
            res
        );
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(typeof(List<TankResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Retrieve tanks by station ID.",
        Description = "Retrieve a list of tanks that are belong to the given station."
    )]
    public static async Task<IResult> GetTankByStationId([FromRoute] int id, ITankRepository tankRepository)
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
    public static IResult GetJWKs(IJWKsService jWKs)
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
    public static async Task<IResult> Login(HttpContext context, [FromBody] LoginRequest body, IUserRepository userRepository, IHasher hasher, IJWTService jwt, IAntiforgery antiforgery)
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
                    catch (PostgresException e){
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
                        new CookieOptions{
                            Path = "/",
                            HttpOnly = false,
                            SameSite = SameSiteMode.None,
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
        catch (InvalidOperationException)
        {   
            return TypedResults.NotFound();
        }
    }
    [Authorize]
    [Permission("user")]
    [RequireAntiforgeryToken]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Delete station",
        Description = "Delete a station from a given id."
    )]
    public static async Task<IResult> DeleteStationFromId([FromRoute] int id, IStationRepository stationRepository)
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

    [Authorize]
    [Permission("user")]
    [RequireAntiforgeryToken]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [SwaggerOperation(
        Summary = "Update station",
        Description = "Update name, address of a station from a given id."
    )]
    public static async Task<IResult> UpdateStationFromId([FromRoute] int id, [FromBody] StationUpdateRequest body, IStationRepository stationRepository, IJWTService jWTService)
    {
        int res;
        try
        {
            res = await stationRepository.UpdateAsync(new Station { StationId = id, Name = body.Name, Address = body.Address});
        }
        catch (PostgresException)
        {
            return TypedResults.InternalServerError();
        }
        if (res == 1)
            return TypedResults.Ok();
        return TypedResults.NotFound();
    }

    [Authorize]
    [Permission("user")]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(List<StationResponse>), 200)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Obtain stations.",
        Description = "Obtain a list of all stations."
    )]
    public static async Task<IResult> GetStations(IStationRepository stationRepository)
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


    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    [Produces("application/json")]
    [SwaggerOperation(
        Summary = "Refresh access token.",
        Description = "Return a new access token if the refresh token is valid and not expired to the user."
    )]
    public static async Task<IResult> RefreshJWT(ILogger<object> logger, [FromBody] TokenRequest tokenRequest, IUserRepository userRepository, IJWTService jWTService, IHasher hasher)
    {
        try
        {
            var claims = new 
            JwtSecurityTokenHandler().
            ReadJwtToken(
               tokenRequest.Token 
            ).
            Claims;
            int userId = Convert.ToInt32(claims.First(e => e.Type == ClaimTypes.Sid).Value);
            string username = claims.First(e => e.Type == ClaimTypes.Name).Value;
            var user = await userRepository.GetUserTokenAsync(new User
            {
                UserId = userId,
                Username = username
            });
            if (
                DateTime.UtcNow.CompareTo(user.TokenExpiredTime) > 0 ||
                !hasher.Verify(user, tokenRequest.RefreshToken + user.TokenPadding!, user.RefreshToken!)
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
            logger.LogError(ex.StackTrace);
            if (ex is PostgresException)
            {
                return TypedResults.Unauthorized();
            }
            return TypedResults.InternalServerError();
        }

    }
}