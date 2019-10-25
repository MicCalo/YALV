using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YALV.Core.Domain;
using YALV.Core.Model;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Commands;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.IO;

namespace YALV.DefaultPlugins
{
    public class ResetFileCommandPlugin : ICommandPlugin
    {
        private static IYalvPluginInformation _info = new YalvPluginInformation("Reset/Clear file button", "Adds a toolbutton which clears the selected file(s)", "(c) 2019 Michel Calonder", new System.Version(1, 0, 0));
        private readonly ICommand _command;
        private readonly  IPluginContext _context;

        public bool IsEnabled
        {
            get { return true; }
        }

        public IYalvPluginInformation Information { get { return _info; } }

        public ResetFileCommandPlugin(IPluginContext context)
        {
            _command = new SimpleActionCommand(Execute);
            _context = context;
        }

        private void Execute(object parameter)
        {
            IDataAccess dataAccess = _context.DataAccess;
            if (dataAccess.IsFileSelectionEnabled)
            {
                IReadOnlyList<FileItem> files = dataAccess.FileList.Where(x => x.Checked).ToList();
                string msg = string.Join(", ", files.Select(x => x.FileName).ToArray());
                if (MessageBox.Show(string.Format("Do you want to reset the following files: {0}", msg), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    dataAccess.Items.Clear();
                    foreach (FileItem f in files)
                    {
                        ResetFile(f);
                    }
                }
            }
            else
            {
                if (MessageBox.Show(string.Format("Do you want to reset file {0}", dataAccess.SelectedFile.FileName), "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    dataAccess.Items.Clear();
                    ResetFile(dataAccess.SelectedFile);
                }
            }
        }

        private void ResetFile(FileItem item)
        {
            File.WriteAllText(item.Path, String.Empty);
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
            get { return "CLEAR FILE"; }
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
