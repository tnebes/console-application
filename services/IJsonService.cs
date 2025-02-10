#region

using console_app.Models;

#endregion

namespace console_app.services;

public interface IJsonService
{
    T CreateEntity<T>(T entity) where T : Entity;
    T? LoadEntity<T>(long id) where T : Entity;
    List<T> LoadEntities<T>() where T : Entity;
    T UpdateEntity<T>(T entity, long id) where T : Entity;
    bool DeleteEntity<T>(long id) where T : Entity;
    int GetMaxId<T>() where T : Entity;
}