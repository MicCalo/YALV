using System.IO;
using YALV.Core.Model;

namespace YALV.Core.Plugins
{
    public interface IPluginContext
    {
        IDataAccess DataAccess { get; set; }
        DirectoryInfo PluginDirectory { get; }
    }

    public class PluginContext : IPluginContext
    {
        private readonly DirectoryInfo _pluginDirectory;

        public PluginContext(DirectoryInfo pluginDirectory)
        {
            _pluginDirectory = pluginDirectory;
        }

        public IDataAccess DataAccess { get; set; }

        public DirectoryInfo PluginDirectory
        {
            get { return _pluginDirectory; }
        }
    }
}
