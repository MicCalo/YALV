using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using YALV.Common.Converters;
using YALV.Core.Domain;
using YALV.Core.Filters;
using YALV.Properties;

namespace YALV.Common
{
    public class FilteredGridManager
        : FilteredGridManagerBase
    {
        public FilteredGridManager(DataGrid dg, Panel txtSearchPanel, Action<LogItemProperty, Control> filterChanged)
            : base(dg, txtSearchPanel, filterChanged)
        {
            _centerCellStyle = Application.Current.FindResource("CenterDataGridCellStyle") as Style;
            _adjConv = new AdjustValueConverter();
        }

        #region Private Properties

        private Style _centerCellStyle;
        private AdjustValueConverter _adjConv;

        #endregion

        #region Public Methods
        

        public void BuildDataGrid(IList<ColumnItem> columns)
        {
            if (_dg == null)
                return;

            if (columns != null)
            {
                foreach (ColumnItem item in columns)
                {
                    DataGridColumn col;
                    if (item.Field.Equals("IsMarked"))
                    {
                        DataGridCheckBoxColumn cbCol = new DataGridCheckBoxColumn();
                        cbCol.IsReadOnly = false;
                        col = cbCol;
                        cbCol.Header = item.Header;
                        Binding bind = new Binding(item.Field) { Mode = BindingMode.TwoWay };
                        cbCol.Binding = bind;
                    }
                    else
                    {
                        DataGridTextColumn textCol = new DataGridTextColumn();
                        col = textCol;
                        textCol.Header = item.Header;
                        Binding bind = new Binding(item.Field) { Mode = BindingMode.OneWay };
                        bind.ConverterCulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Resources.CultureName);
                        if (!String.IsNullOrWhiteSpace(item.StringFormat))
                            bind.StringFormat = item.StringFormat;
                        textCol.Binding = bind;
                    }
                    if (item.Alignment == CellAlignment.CENTER && _centerCellStyle != null)
                        col.CellStyle = _centerCellStyle;
                    if (item.MinWidth != null)
                        col.MinWidth = item.MinWidth.Value;
                    if (item.Width != null)
                        col.Width = item.Width.Value;

                    col.Visibility = item.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                    //Add column to datagrid
                    _dg.Columns.Add(col);

                    if (_txtSearchPanel != null)
                    {
                        Binding widthBind = new Binding()
                        {
                            Path = new PropertyPath("ActualWidth"),
                            Source = col,
                            Mode = BindingMode.OneWay,
                            Converter = _adjConv,
                            ConverterParameter = "-2"
                        };

                        LogItemProperty prop = (LogItemProperty)Enum.Parse(typeof(LogItemProperty), item.Field);
                        IPropertyFilterInfo info = FilterManager.CreateFilterInfo(prop, _filterChanged);
                        info.Control.SetBinding(TextBox.WidthProperty, widthBind);
                        info.Control.ToolTip = String.Format(Resources.FilteredGridManager_BuildDataGrid_FilterTextBox_Tooltip, item.Header);
                        info.Control.Tag = info.Control.ToolTip.ToString().ToLower();
                        _txtSearchPanel.Children.Add(info.Control);

                        Filter.Add(prop, info.Filter);
                    }
                }
            }

            _dg.ColumnReordered += OnColumnReordered;
        }

        private MenuItem CreateMenuItem(string txt, string tag)
        {
            MenuItem result = new MenuItem(){Header = txt};
            if (tag != null)
            {
                result.Tag = tag;
                result.Click += HandleMenuItemClick;

            }
            return result;
        }

        public void BuildHeaderCtxMenu(IList<ColumnItem> columns, ContextMenu ctxMenu)
        {
            ctxMenu.Items.Add(CreateMenuItem("Hide", "-hide-"));
            ctxMenu.Items.Add(new Separator());
            foreach (ColumnItem column in columns)
            {
                MenuItem i = CreateMenuItem("Show " + column.Header, column.Header);
                i.Visibility = column.IsVisible ? Visibility.Collapsed : Visibility.Visible;
                ctxMenu.Items.Add(i);
            }
        }


        #endregion

        #region Private methods

        
        private void HandleMenuItemClick(object sender, RoutedEventArgs  o)
        {
            MenuItem item = (MenuItem) sender;
            ContextMenu menu = (ContextMenu) item.Parent;

            string tag = (string)item.Tag;
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            if ("-hide-".Equals(tag))
            {
                DataGridColumnHeader hdr = (DataGridColumnHeader) menu.PlacementTarget;
                hdr.Column.Visibility = Visibility.Collapsed;
                MenuItem i = Get(menu, hdr.Content);
                if (i != null)
                {
                    i.Visibility = Visibility.Visible;
                }
            }
            else
            {
                foreach (DataGridColumn col in _dg.Columns)
                {

                    DataGridColumnHeader hdr = GetHeader(col, _dg);
                    if (hdr.Content.Equals(tag))
                    {
                        col.Visibility = Visibility.Visible;
                        break;
                    }
                }

                item.Visibility = Visibility.Collapsed;
            }
        }

        private DataGridColumnHeader GetHeader(DataGridColumn column, DependencyObject reference)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(reference); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(reference, i);

                DataGridColumnHeader colHeader = child as DataGridColumnHeader;
                if ((colHeader != null) && (colHeader.Column == column))
                {
                    return colHeader;
                }

                colHeader = GetHeader(column, child);
                if (colHeader != null)
                {
                    return colHeader;
                }
            }

            return null;
        }

        private MenuItem Get(ContextMenu menu, object tag)
        {
            foreach (object o in menu.Items)
            {
                MenuItem item = o as MenuItem;
               
                if (item != null)
                {
                    if (item.Tag.Equals(tag))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        private void OnColumnReordered(object sender, DataGridColumnEventArgs dataGridColumnEventArgs)
        {
            if (dataGridColumnEventArgs.Column == null || !(dataGridColumnEventArgs.Column is DataGridBoundColumn))
                return;

            Binding colBind = ((DataGridBoundColumn)dataGridColumnEventArgs.Column).Binding as Binding;
            if (colBind == null || colBind.Path == null)
                return;

            string field = colBind.Path.Path;
            if (String.IsNullOrWhiteSpace(field))
                return;

            int displayOrder = dataGridColumnEventArgs.Column.DisplayIndex;
            string textBoxName = getTextBoxName(field);

            TextBox textBox = (from tb in _txtSearchPanel.Children.OfType<TextBox>()
                               where tb.Name == textBoxName
                               select tb).FirstOrDefault<TextBox>();

            if (textBox == null)
                return;

            _txtSearchPanel.Children.Remove(textBox);
            _txtSearchPanel.Children.Insert(displayOrder, textBox);
        }

        #endregion
    }
}
