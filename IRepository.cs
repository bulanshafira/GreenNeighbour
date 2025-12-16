namespace WasteManagementConsole.Data;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    T? GetById(int id);
    List<T> GetAll();
    void Update(T entity);
    void Delete(int id);
}