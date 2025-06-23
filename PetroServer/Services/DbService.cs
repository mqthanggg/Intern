public class DBWrite : IDbWrite{
    public NpgsqlDataSource DataSource{get; set;}
    public DBWrite(){
        DataSource = NpgsqlDataSource.Create(Env.GetString("DBWRITE_CONNECTION_STRING"));
    }
}

public class DBRead : IDbRead{
    public NpgsqlDataSource DataSource{get; set;}
    public DBRead(){
        DataSource = NpgsqlDataSource.Create(Env.GetString("DBREAD_CONNECTION_STRING"));
    }
}