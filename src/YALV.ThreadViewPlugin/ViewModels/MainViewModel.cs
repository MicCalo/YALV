using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using YALV.Core.Domain;

namespace YALV.ThreadViewPlugin.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private List<ThreadViewModel> threads;
        private List<HeaderViewModel> headers = new List<HeaderViewModel>();
        private List<ItemViewModel> entries = new List<ItemViewModel>();
        private DataGrid dataGrid;

        public HeaderViewModel GrpHeader { get; private set; }
        public HeaderViewModel IdHeader { get; private set; }
        public HeaderViewModel TimeHeader { get; private set; }

        public MainViewModel(IEnumerable<LogItem> items)
        {
            Dictionary<string, ThreadViewModel> threadDict = new Dictionary<string, ThreadViewModel>();
            GrpHeader = new HeaderViewModel("", 10, 15);
            IdHeader = new HeaderViewModel("Id", 10, 25);
            TimeHeader = new HeaderViewModel("Time", 20, 75);
            ItemViewModel previous = null;
            foreach (LogItem item in items)
            {
                ThreadViewModel t;
                if (!threadDict.TryGetValue(item.Thread, out t))
                {
                    t = new ThreadViewModel(item.Thread, this);
                    threadDict.Add(item.Thread, t);
                }

                ItemViewModel e = new ItemViewModel(item, previous, t);
                entries.Add(e);
                previous = e;
            }
            threads = threadDict.Values.ToList();
            headers.Add(GrpHeader);
            headers.Add(IdHeader);
            headers.Add(TimeHeader);
            headers.AddRange(threads);
        }

        public void SetGridView(DataGrid dg)
        {
            dataGrid = dg;

            foreach (HeaderViewModel t in headers)
            {
                DataGridColumnHeader header = new DataGridColumnHeader() { DataContext = t, Content = t.Name };
                header.SizeChanged += HandleHeaderSizeChanged;

                DataGridTextColumn col = new DataGridTextColumn() { Header = header };
                
                col.MinWidth = t.MinWith;
                col.Width = t.Width;

                dg.Columns.Add(col);
            }

            dg.ColumnReordered += HandleColumnReordered;
            UpdateWidths();
        }

        private void HandleColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            UpdateWidths();
        }

        private void UpdateWidths()
        {
            double x = -3;
            foreach (DataGridColumn col in dataGrid.Columns.OrderBy(c => c.DisplayIndex))
            {
                DataGridColumnHeader hdr = col.Header as DataGridColumnHeader;
                if (hdr != null)
                {
                    HeaderViewModel t = hdr.DataContext as HeaderViewModel;
                    t.Width = col.ActualWidth;
                    t.Left = x;
                }
                x += col.ActualWidth;
            }
            FirePropertyChanged("Items");
        }

        private void HandleHeaderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateWidths();
        }

        public IReadOnlyList<ThreadViewModel> Threads { get { return threads.ToList(); } }

        public IReadOnlyList<ItemViewModel> Items { get { return entries.AsReadOnly(); } }
    }
}
