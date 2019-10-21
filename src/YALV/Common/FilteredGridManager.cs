using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
                        //  Control ctrl = info.Control;
                        info.Control.SetBinding(TextBox.WidthProperty, widthBind);
                        info.Control.ToolTip = String.Format(Resources.FilteredGridManager_BuildDataGrid_FilterTextBox_Tooltip, item.Header);
                        info.Control.Tag = info.Control.ToolTip.ToString().ToLower();
                        //RegisterControl(_txtSearchPanel, info.Control.Name, info.Control);
                        _txtSearchPanel.Children.Add(info.Control);

                        Filter.Add(prop, info.Filter);
                    }
                }
            }

            _dg.ColumnReordered += OnColumnReordered;
        }


        #endregion

        #region Private methods

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

        private void RegisterControl(FrameworkElement element, string controlName, Control control)
        {
            if (element.FindName(controlName) != null)
            {
                element.UnregisterName(controlName);
            }
            element.RegisterName(controlName, control);
        }

        #endregion
    }
}
