public interface IUserRepository<T> : IRepository<T>
{
    Task<User?> UserExist(string email);
}