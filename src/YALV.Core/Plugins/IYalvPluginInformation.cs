using System;

namespace YALV.Core.Plugins
{
    public interface IYalvPluginInformation
    {
        string Name { get; }
        string Description { get; }
        string Copyright { get; }
        Version Version { get; }
    }
}
