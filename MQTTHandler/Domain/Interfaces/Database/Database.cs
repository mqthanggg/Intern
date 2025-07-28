public interface IDbWriteConnection{
    NpgsqlConnection CreateConnection();
}
public interface IDbReadConnection{
    NpgsqlConnection CreateConnection();
}