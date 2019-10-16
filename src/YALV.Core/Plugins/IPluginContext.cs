using YALV.Core.Model;

namespace YALV.Core.Plugins
{
    public interface IPluginContext
    {
        IDataAccess DataAccess { get; set; }
    }

    public class PluginContext : IPluginContext
    {
        public IDataAccess DataAccess { get; set; }
    }
}
