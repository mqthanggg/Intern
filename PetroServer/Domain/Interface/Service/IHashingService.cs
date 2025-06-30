public interface IHasher{
    (string,string) Hash(object obj, string password);
    bool Verify(object obj, string inPassword, string hashedPassword);
}