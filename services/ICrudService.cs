#region

#region

using console_app.Models;

#endregion

namespace console_app.Services;

#endregion

public interface ICrudService
{
    void Create();
    void GetAll();
    void GetById();
    void Update();
    void Delete();
}

public interface ICrudService<T> : ICrudService where T : Entity
{
}