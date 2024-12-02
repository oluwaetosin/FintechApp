public interface IRepository<T>
{
    T? GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task<Guid> Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}