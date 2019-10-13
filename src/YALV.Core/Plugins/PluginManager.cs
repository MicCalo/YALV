using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YALV.Core.Plugins
{
    internal class PluginManager
    {
        private static PluginManager instance;
        private static readonly Type iYalvPluginType = typeof(IYalvPlugin);

        private static readonly string[] excludedDlls = new string[] { "YALV.Core.dll", "System.Data.SQLite.dll" };
        private readonly List<IYalvPlugin> plugins = new List<IYalvPlugin>();

        private PluginManager()
        {
            DirectoryInfo runningDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            foreach (FileInfo file in runningDir.GetFiles("*.dll"))
            {
                if (!excludedDlls.Contains(file.Name))
                {
                    Assembly assembly = Assembly.LoadFile(file.FullName);
                    SearchAssembly(assembly);
                }
            }

            SearchAssembly(this.GetType().Assembly);
            plugins.Sort((a, b) => (a.Priority - b.Priority));
        }

        private void SearchAssembly(Assembly assembly)
        {
            foreach (Module module in assembly.Modules)
            {
                foreach (Type t in module.GetTypes())
                {
                    if (t.IsClass && !t.IsAbstract && iYalvPluginType.IsAssignableFrom(t))
                    {
                        plugins.Add((IYalvPlugin)Activator.CreateInstance(t));
                    }
                }
            }
        }

        public IReadOnlyList<T> GetPlugins<T>() where T: IYalvPlugin
        {
            return plugins.OfType<T>().ToList();
        }

        public static PluginManager Instance { get
            {
                if (instance == null){
                    instance = new PluginManager();
                }
                return instance;
            }
        }
    }
}
