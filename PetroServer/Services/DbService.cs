public class DbWriteConnection : IDbWriteConnection{
    public NpgsqlConnection CreateConnection(){
        return new NpgsqlConnection(Env.GetString("DBWRITE_CONNECTION_STRING"));
    }
}

public class DbReadConnection : IDbReadConnection{
    public NpgsqlConnection CreateConnection(){
        return new NpgsqlConnection(Env.GetString("DBREAD_CONNECTION_STRING"));    
    }
}
