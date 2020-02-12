using System.Windows.Input;

namespace YALV.Core.Plugins.Commands
{
    public interface ICommandAncestor
        : ICommand
    {
        void OnCanExecuteChanged();
    }
}
