public class DbWriteConnection : IDbWriteConnection{
    private readonly string ConnectionString;
    public DbWriteConnection(
    ){
        ConnectionString = Env.GetString("DBWRITE_CONNECTION_STRING");
    }
    public NpgsqlConnection CreateConnection(){
        return new NpgsqlConnection(ConnectionString);
    }
}