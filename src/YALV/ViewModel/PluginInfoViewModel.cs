using System.Windows;
using YALV.Core.Plugins;

namespace YALV.ViewModel
{
    public class PluginInfoViewModel
    {
        private readonly IYalvPlugin plugin;

        public PluginInfoViewModel(IYalvPlugin plugin)
        {
            this.plugin = plugin;
        }

        public bool IsEnabled { get { return plugin.IsEnabled; } }
        public Visibility IsDisabledVisibility { get { return plugin.IsEnabled ? Visibility.Collapsed : Visibility.Visible; } }
        public string Name { get { return plugin.Information.Name; } }
        public string Description { get { return plugin.Information.Description; } }
        public string Copyright { get { return plugin.Information.Copyright; } }
        public string Version { get { return plugin.Information.Version.ToString(); } }
        public string ClassName { get { return plugin.GetType().Name; } }
        public string Origin { get { return plugin.GetType().Assembly.ToString(); } }
    }
}
