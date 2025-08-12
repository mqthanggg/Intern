public static class UserQuery{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string InsertUser = $@"
        INSERT INTO {Schema}.user(
            username, 
            password, 
            role,
            padding, 
            refresh_token, 
            token_padding,
            token_expired_time,
            created_by,
            created_date,
            last_modified_by,
            last_modified_date
        ) VALUES (
            @Username, 
            @Password, 
            @Role,
            @Padding,
            @RefreshToken,
            @TokenPadding,
            @TokenExpiredTime,
            @CreatedBy,
            now(),
            @LastModifiedBy,
            now()
        )
    ";
    public static readonly string SelectUser = $@"
        SELECT 
            user_id, 
            username, 
            role,
            password, 
            padding 
        FROM {Schema}.user
    ";
    public static readonly string SelectUserById = $@"
        SELECT
            user_id, 
            username, 
            role,
            password, 
            padding 
        FROM {Schema}.user
        WHERE 
            user_id = @UserId
    ";
    public static readonly string UpdateUser = $@"
        UPDATE {Schema}.user
        SET
            refresh_token = @RefreshToken,
            token_padding = @TokenPadding,
            token_expired_time = @TokenExpiredTime,
            last_modified_by = @LastModifiedBy,
            last_modified_date = now()
        WHERE
            user_id = @UserId
    ";
    public static readonly string DeleteUser = $@"
        DELETE FROM {Schema}.user
        WHERE 
            user_id = @UserId
    ";
    public static readonly string GetUserByUsername = $@"
        SELECT
            user_id, 
            username, 
            role,
            password, 
            padding 
        FROM {Schema}.user
        WHERE 
            username = @Username
    ";
    public static readonly string GetUserToken = $@"
        SELECT 
            refresh_token, 
            token_padding, 
            token_expired_time 
        FROM {Schema}.user
        WHERE 
            user_id = @UserId
        AND
            username = @Username
    "; 
    public static readonly string GetUserWithActive = $@"
        SELECT
            user_id,
            username,
            role,
            (
                refresh_token IS NOT NULL AND
                token_expired_time > now()
            ) AS active
        FROM {Schema}.user
        INNER JOIN (
            SELECT 
                user_id
            FROM {Schema}.user
            WHERE
                (@Username IS NULL OR username LIKE @Username)
            AND
                (@Role IS NULL OR role = @Role)
            AND
                (
                    @Active IS NULL OR
                    (
                        @Active = 1 AND
                            refresh_token IS NOT NULL AND
                            token_expired_time > now()
                    ) OR
                    (
                        @Active = 0 AND
                            (
                                refresh_token IS NULL OR
                                token_expired_time <= now()
                            )
                    )
                )
            ORDER BY
                user_id
            LIMIT @Limit
            OFFSET @Offset
        ) AS user2 USING (user_id)
    ";
    public static readonly string UpdateUserPassword = $@"
        UPDATE {Schema}.user
        SET
            password = @Password,
            padding = @Padding,
            refresh_token = NULL,
            token_padding = NULL,
            token_expired_time = NULL,
            last_modified_by = @LastModifiedBy,
            last_modified_date = now()
        WHERE
            user_id = @UserId
    ";
    public static readonly string GetUserCount = $@"
        SELECT COUNT(1) FROM {Schema}.user
        WHERE
            (@Username IS NULL OR username LIKE @Username)
        AND
            (@Role IS NULL OR role = @Role)
        AND
            (
                @Active IS NULL OR
                (
                    @Active = 1 OR 
                        refresh_token IS NOT NULL AND
                        token_expired_time > now()
                ) OR
                (
                    @Active = 0 OR 
                        refresh_token IS NULL OR
                        token_expired_time <= now()
                )
            )
    ";
}