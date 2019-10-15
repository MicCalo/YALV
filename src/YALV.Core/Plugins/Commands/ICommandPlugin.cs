using System;
using System.Windows.Input;
using System.Windows.Media;

namespace YALV.Core.Plugins.Commands
{
    [Flags]
    public enum CommandPluginLocation
    {
        MainToolBar = 1
    }

    public interface ICommandPlugin : IYalvPlugin
    {
        CommandPluginLocation Location { get; }

        string ToolTip { get; }
        ImageSource ImageSource { get; }
        string Text { get; }
        ICommand Command { get; }
    }
}
