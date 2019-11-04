using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Commands;

namespace YALV.Common
{
    /// <summary>
    /// Interaction logic for MainToolbar.xaml
    /// </summary>
    public partial class MainToolbar : ToolBar
    {
        public MainToolbar()
        {
            InitializeComponent();

            this.Loaded += delegate(object sender, RoutedEventArgs e)
            {
                ToolBar toolBar = sender as ToolBar;
                if (toolBar != null)
                {
                    FrameworkElement overflowGrid = (FrameworkElement)toolBar.Template.FindName("OverflowGrid", toolBar);
                    if (overflowGrid != null)
                        overflowGrid.Visibility = Visibility.Collapsed;


                    IEnumerable< ICommandPlugin> commandPlugins = PluginManager.Instance.GetPlugins<ICommandPlugin>().Where(x=>x.Location.HasFlag(CommandPluginLocation.MainToolBar));

                    foreach(ICommandPlugin cPlug in commandPlugins)
                    {
                        Button btn = new Button();
                        btn.Command = cPlug.Command;
                        btn.ToolTip = cPlug.ToolTip;

                        StackPanel stackPanel = new StackPanel();
                        stackPanel.Children.Add(new Image() { Source = cPlug.ImageSource });
                        stackPanel.Children.Add(new TextBlock() { Text = cPlug.Text });
                        btn.Content = stackPanel;

                        AddChild(btn);
                    }
                }
            };
        }
    }
}
