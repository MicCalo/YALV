using System;
using System.Windows.Input;

namespace YALV.Core.Plugins.Commands
{
    public class SimpleActionCommand : ICommand
    {
        private readonly Action<object> executeAction;

        public SimpleActionCommand(Action<object> executeAction)
        {
            this.executeAction = executeAction;
        }

        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return executeAction != null;
        }

        public void Execute(object parameter)
        {
           if (executeAction != null)
            {
                executeAction(parameter);
            }
        }
    }
}
