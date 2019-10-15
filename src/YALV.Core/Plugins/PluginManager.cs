using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YALV.Core.Model;

namespace YALV.Core.Plugins
{
    public class PluginManager
    {
        private static PluginManager instance;
        private static readonly Type iYalvPluginType = typeof(IYalvPlugin);

        private readonly List<IYalvPlugin> plugins = new List<IYalvPlugin>();

        private PluginManager(IMainModel mainModel)
        {
            Dictionary<Type, object> potentialParams = new Dictionary<Type, object>();
            potentialParams.Add(mainModel.GetType(), mainModel);

            SearchAssembly(this.GetType().Assembly, potentialParams);

            DirectoryInfo runningDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"));
            foreach (FileInfo file in runningDir.GetFiles("*.dll"))
            {
                Assembly assembly = Assembly.LoadFile(file.FullName);
                SearchAssembly(assembly, potentialParams);
            }

            plugins.Sort((a, b) => (a.Priority - b.Priority));
        }

        private void SearchAssembly(Assembly assembly, Dictionary<Type, object> potentialParams)
        {
            foreach (Module module in assembly.Modules)
            {
                foreach (Type t in module.GetTypes())
                {
                    if (t.IsClass && !t.IsAbstract && iYalvPluginType.IsAssignableFrom(t))
                    {
                        plugins.Add(Create(t, potentialParams));
                    }
                }
            }
        }

        private IYalvPlugin Create(Type type, Dictionary<Type, object> potentialParams)
        {
            ConstructorInfo[] ctors = type.GetConstructors();
            object[] parameters = GetParams(ctors, potentialParams);
            if (parameters != null)
            {
                return (IYalvPlugin)Activator.CreateInstance(type, parameters);
            }

            throw new InvalidOperationException(string.Format("No suiting counstructor found for type {0}",  type.Name));
        }

        private object[] GetParams(ConstructorInfo[] ctors, Dictionary<Type, object> potentialParams)
        {
            foreach(ConstructorInfo ctor in ctors)
            {
                object[] args = GetParams(ctor, potentialParams);
                if (args != null)
                {
                    return args;
                }
            }
            return null;
        }
        private object[] GetParams(ConstructorInfo ctor, Dictionary<Type, object> potentialParams)
        {
            List<object> result = new List<object>();
            foreach(ParameterInfo p in ctor.GetParameters())
            {
                object o;
                if (!TryGetParam(potentialParams, p.ParameterType, out o))
                {
                    return null;
                }
                result.Add(o);
            }
            return result.ToArray();
        }

        private bool TryGetParam(Dictionary<Type, object> potentialParams, Type paramType, out object obj)
        {
            foreach(KeyValuePair<Type, object> kv in potentialParams)
            {
                if (paramType.IsAssignableFrom(kv.Key))
                {
                    obj = kv.Value;
                    return true;
                }
            }

            obj = null;
            return false;
        }

        public IReadOnlyList<T> GetPlugins<T>() where T: IYalvPlugin
        {
            return plugins.OfType<T>().ToList();
        }

        public static PluginManager Instance { get
            {
                if (instance == null){
                    instance = new PluginManager(MainModelAccess.Instance.MainModel);
                }
                return instance;
            }
        }
    }
}
