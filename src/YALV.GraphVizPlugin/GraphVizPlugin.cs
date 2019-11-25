using log4net;
using Microsoft.Msagl.GraphViewerGdi;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YALV.Core.Domain;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Commands;
using YALV.GraphVizPlugin.Model;

namespace YALV.GraphVizPlugin
{
    public class GraphVizPlugin : ICommandPlugin
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GraphVizPlugin));

        private static Regex regex = new Regex(@"^State change: (?<from>[\w ]*) -> (?<to>[\w ]*)$");
        private static IYalvPluginInformation _info = new YalvPluginInformation("Graph Visuzalization", "Extracts a graph out of state transition messages", "(c) 2019 Michel Calonder", new System.Version(0, 1, 0));
        private readonly ICommand _command;
        private readonly IPluginContext _context;

        public bool IsEnabled
        {
            get { return true; }
        }

        public IYalvPluginInformation Information { get { return _info; } }

        public GraphVizPlugin(IPluginContext context)
        {
            _command = new SimpleActionCommand(Execute);
            _context = context;
        }

        private void Execute(object parameter)
        {
            GraphBuilder builder = new GraphBuilder();
            foreach (LogItem item in _context.DataAccess.Items)
            {
                Match m = regex.Match(item.Message);
                if (m.Success)
                {
                    string from = m.Groups["from"].Value;
                    string to = m.Groups["to"].Value;
                    builder.AddTransition(from, to, item.Id);
                }
            }

            GViewer viewer = builder.CreateViewer();

            Window win = new Window();
            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = viewer;
            win.Content = host;
            win.Show();
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
            get { return "Create a graph"; }
        }

        public ImageSource ImageSource
        {
            get
            {
                Uri oUri = new Uri("pack://application:,,,/" + this.GetType().Assembly.GetName() + ";component/Resources/Graph.png", UriKind.RelativeOrAbsolute);
                return BitmapFrame.Create(oUri);
            }
        }

        public string Text
        {
            get { return "VISUALIZE GRAPH"; }
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
