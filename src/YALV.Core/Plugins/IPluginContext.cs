using System.IO;
using YALV.Core.Model;
using YALV.Core.Settings;

namespace YALV.Core.Plugins
{
    public interface IPluginContext
    {
        IDataAccess DataAccess { get; set; }
        DirectoryInfo PluginDirectory { get; }
        IConfiguration Configuration { get; }
    }

    public class PluginContext : IPluginContext
    {
        private readonly DirectoryInfo _pluginDirectory;

        public PluginContext(DirectoryInfo pluginDirectory)
        {
            _pluginDirectory = pluginDirectory;
            Configuration = new Configuration();
        }

        public IDataAccess DataAccess { get; set; }

        public DirectoryInfo PluginDirectory
        {
            get { return _pluginDirectory; }
        }

        public IConfiguration Configuration { get; private set; }
    }
}
