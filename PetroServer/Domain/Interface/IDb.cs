public interface IDbWrite{
    NpgsqlDataSource DataSource{get; set;}
}

public interface IDbRead{
    NpgsqlDataSource DataSource{get; set;}
}