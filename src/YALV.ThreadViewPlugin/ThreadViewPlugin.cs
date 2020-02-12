using log4net;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Commands;
using YALV.ThreadPlugin.Views;
using YALV.ThreadViewPlugin.ViewModels;

namespace YALV.GraphVizPlugin
{
    public class ThreadViewPlugin : ICommandPlugin
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ThreadViewPlugin));

        private static IYalvPluginInformation _info = new YalvPluginInformation("Thread Visuzalization", "Shows messages by each thread", "(c) 2019 Michel Calonder", new System.Version(0, 1, 0));
        private readonly ICommand _command;
        private readonly IPluginContext _context;

        public bool IsEnabled
        {
            get { return false; }
        }

        public IYalvPluginInformation Information { get { return _info; } }

        public ThreadViewPlugin(IPluginContext context)
        {
            _command = new CommandRelay(Execute, CommandCanExecute);
            _context = context;
        }

        private object Execute(object parameter)
        {
            MainViewModel vm = new MainViewModel(_context.DataAccess.Items);

            MainWindow win = new MainWindow();
            win.DataContext = vm;
            vm.SetGridView(win.DataGrid);
            win.Show();
            /*
            Window win = new Window();
            ThreadViewPanel panel = new ThreadViewPanel();
            panel.DataContext = vm;
            win.Content = panel;
            win.Show();
            */

            return null;
        }

        private bool CommandCanExecute(object parameter)
        {
            return true;
        }

        public CommandPluginLocation Location
        {
            get { return CommandPluginLocation.MainToolBar; }
        }

        public int Priority
        {
            get { return 1000; }
        }

        public string ToolTip
        {
            get { return "show by threads"; }
        }

        public ImageSource ImageSource
        {
            get
            {
                Uri oUri = new Uri("pack://application:,,,/" + this.GetType().Assembly.GetName() + ";component/Resources/threads.png", UriKind.RelativeOrAbsolute);
                return BitmapFrame.Create(oUri);
            }
        }

        public string Text
        {
            get { return "VISUALIZE BY THREAD"; }
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
