namespace YALV.Core.Settings
{
    public interface IConfiguration
    {
        object Get(string setting);
        T Get<T>(string setting);
        void Set(string setting, object value);
        void Save();
    }
}
