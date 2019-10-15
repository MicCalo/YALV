using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YALV.Core.Model;
using YALV.Core.Plugins.Commands;

namespace OmnisPluginsForYalv
{
    public class ResetFileCommandPlugin : ICommandPlugin
    {
        private readonly ICommand _command = new SimpleActionCommand(Execute);

        public ResetFileCommandPlugin(IMainModel model)
        {

        }
        private static void Execute(object parameter)
        {

        }

        public CommandPluginLocation Location
        {
            get { return CommandPluginLocation.MainToolBar; }
        }

        public int Priority
        {
            get { return 0; }
        }

        public string ToolTip
        {
            get { return "Clears the current File"; }
        }

        public ImageSource ImageSource
        {
            get
            {
                Uri oUri = new Uri("pack://application:,,,/" + this.GetType().Assembly.GetName() + ";component/Resources/bin.png", UriKind.RelativeOrAbsolute);
                return BitmapFrame.Create(oUri);
            }
        }

        public string Text
        {
            get { return "Clear file"; }
        }

        public ICommand Command
        {
            get
            {
                return _command;
            }
        }
    }
}
