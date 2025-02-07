using console_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_app.Services
{
    public interface IJsonService<T> where T : Entity
    {
        T CreateEntity(T entity);
        T LoadEntity(long id);
        List<T> LoadEntities();
        T UpdateEntity(T entity, long id);
        Boolean DeleteEntity(long id);
    }
}
