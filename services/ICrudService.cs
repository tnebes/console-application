#region

using console_app.Models;

#endregion

namespace console_app.services;

public interface ICrudService
{
    void Create();
    void GetAll();
    void GetById();
    void Update();
    void Delete();
    void ListAll();
}

public interface ICrudService<T> : ICrudService where T : Entity
{
}