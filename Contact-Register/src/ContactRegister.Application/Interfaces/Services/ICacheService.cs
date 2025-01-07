namespace ContactRegister.Application.Interfaces.Services
{
    public interface ICacheService
    {
        object Get(string key);
        void Set(string key, object value);
        void Remove(string key);
    }
}
