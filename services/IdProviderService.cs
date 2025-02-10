namespace console_app.services;

public interface IIdProviderService
{
    long GetNextId(Type entity);
}