using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace YALV.Core.Settings
{
    public class Configuration : IConfiguration
    {
        private readonly ConfigIo io = new ConfigIo();

        private Dictionary<string, object> settings = new Dictionary<string, object>();

        public Configuration()
        {
            if (!io.TryLoad(settings))
            {
                settings.Clear();
                SetDefaults();
            }
        }

        public void Save()
        {
            io.Save(settings);
        }        

        private void SetDefaults()
        {
            settings.Add("Files.SuitingFileExtensionRegex", new Regex(@"\.log(.\d{1,2})?$"));
            settings.Add("View.ColumnHeaders", new[] { "IsMarked", "Id", "TimeStamp", "Level", "Message", "Thread", "Logger" });
        }

        public object Get(string setting)
        {
            return settings[setting];
        }

        public T Get<T>(string setting)
        {
            return (T)Get(setting);
        }

        public void Set(string setting, object value)
        {
            settings[setting] = value;
        }
    }
}
