public static class UserQuery{
    private static readonly string Schema = Env.GetString("SCHEMA");
    public static readonly string InsertUser = $@"
        INSERT INTO {Schema}.user(
            username, 
            password, 
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
            @Padding,
            @RefreshToken,
            @TokenPadding,
            @TokenExpiredTime,
            @CreatedBy,
            @CreatedDate,
            @LastModifiedBy,
            @LastModifiedDate
        )
    ";
    public static readonly string SelectUser = $@"
        SELECT 
            user_id, 
            username, 
            password, 
            padding 
        FROM {Schema}.user
    ";
    public static readonly string SelectUserById = $@"
        SELECT
            user_id, 
            username, 
            password, 
            padding 
        FROM {Schema}.user
        WHERE 
            @user_id = @UserId
    ";
    public static readonly string UpdateUser = $@"
        UPDATE {Schema}.user
        SET
            refresh_token = @RefreshToken,
            token_padding = @RefreshTokenPadding,
            token_expired_time = now() + INTERVAL '7 days',
            last_modified_by = @LastModifiedBy,
            last_modified_date = @LastModifiedDate
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
            ""password"", 
            padding 
        FROM {Schema}.user
        WHERE 
            username = @Username
    ";
}