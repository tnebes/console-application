#region

using console_app.Models;

#endregion

namespace console_app.Services;

public interface IJsonService<T> where T : Entity
{
    T CreateEntity(T entity);
    T LoadEntity(long id);
    List<T> LoadEntities();
    T UpdateEntity(T entity, long id);
    bool DeleteEntity(long id);
}