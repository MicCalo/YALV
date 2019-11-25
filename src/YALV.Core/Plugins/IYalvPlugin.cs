namespace YALV.Core.Plugins
{
    public interface IYalvPlugin
    {
        int Priority { get; }
        bool IsEnabled { get; }
        IYalvPluginInformation Information { get; }
    }
}
