using System;

namespace YALV.Core.Plugins
{
    public class YalvPluginInformation : IYalvPluginInformation
    {
        private readonly string _name;
        private readonly string _description;
        private readonly string _copyright;
        private readonly Version _version;

        public YalvPluginInformation(string name, string description, string copyright, Version version)
        {
            _name = name;
            _description = description;
            _copyright = copyright;
            _version = version;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }
        public string Copyright
        {
            get { return _copyright; }
        }
        public Version Version
        {
            get { return _version; }
        }

        public override string ToString()
        {
            return string.Format("Name = {0}, Version={1}", _name, _version);
        }
    }
}
