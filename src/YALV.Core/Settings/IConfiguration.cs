using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YALV.Core.Settings
{
    public interface IConfiguration
    {
        object Get(string setting);
        T Get<T>(string setting);
    }

    public class Configuration : IConfiguration
    {
        private Dictionary<string, object> settings = new Dictionary<string, object>();

        public Configuration()
        {
            settings.Add("Files.SuitingFileExtensionRegex", new Regex(@"\.log(.\d{1,2})?$"));
        }

        public object Get(string setting)
        {
            return settings[setting];
        }

        public T Get<T>(string setting)
        {
            return (T)Get(setting);
        }
    }
}
