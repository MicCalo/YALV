using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using YALV.Core.Domain;
using YALV.Core.Filters;
using YALV.Core.Plugins;

namespace YALV.Common
{
    public class FilteredGridManagerBase
        : DisposableObject
    {
        public FilteredGridManagerBase(DataGrid dg, Panel txtSearchPanel, Action<LogItemProperty, Control> filterChanged)
        {
            _dg = dg;
            _txtSearchPanel = txtSearchPanel;
            _filterChanged = filterChanged;
            IsFilteringEnabled = true;
        }

        protected override void OnDispose()
        {
            if (_dg != null)
                _dg.Columns.Clear();
            if (_cvs != null)
            {
                if (_cvs.View != null)
                    _cvs.View.Filter = null;
                BindingOperations.ClearAllBindings(_cvs);
            }
            base.OnDispose();
        }

        #region Private Properties

        protected DataGrid _dg;
        protected Panel _txtSearchPanel;
        protected Action<LogItemProperty, Control> _filterChanged;
        protected CollectionViewSource _cvs;

        private IFilterManager _filterManager;
        protected IFilterManager FilterManager
        {
            get
            {
                if (_filterManager == null)
                {
                    _filterManager = PluginManager.Instance.GetPlugins<IFilterManager>().First();
                }

                return _filterManager;
            }
        }

        private IFilter _filter;
        protected IFilter Filter
        {
            get
            {
                if (_filter == null)
                {
                    _filter = _filterManager.CreateFilter();
                }
                return _filter;
            }
        }

        #endregion

        #region Public Methods

        public virtual void AssignSource(Binding sourceBind)
        {
            if (_cvs == null)
                _cvs = new CollectionViewSource();
            else
                BindingOperations.ClearBinding(_cvs, CollectionViewSource.SourceProperty);

            BindingOperations.SetBinding(_cvs, CollectionViewSource.SourceProperty, sourceBind);
            BindingOperations.ClearBinding(_dg, DataGrid.ItemsSourceProperty);
            Binding bind = new Binding() { Source = _cvs, Mode = BindingMode.OneWay };
            _dg.SetBinding(DataGrid.ItemsSourceProperty, bind);
        }

        public ICollectionView GetCollectionView()
        {
            if (_cvs != null)
            {
                //Assign filter method
                if (_cvs.View != null && _cvs.View.Filter == null)
                {
                    IsFilteringEnabled = false;
                    _cvs.View.Filter = itemCheckFilter;
                    IsFilteringEnabled = true;
                }
                return _cvs.View;
            }
            return null;
        }

        public void ResetSearchTextBox()
        {
            if (_txtSearchPanel != null)
            {
                foreach(Control c in _txtSearchPanel.Children)
                {
                    if (c is CheckBox)
                    {
                        ((CheckBox)c).IsChecked = null;
                    }else
                    {
                        ((TextBox)c).Text = string.Empty;
                    }
                }/*
                //Clear all textbox text
                foreach (string prop in _filterPropertyList)
                {
                    TextBox txt = _txtSearchPanel.FindName(getTextBoxName(prop)) as TextBox;
                    if (txt != null & !string.IsNullOrEmpty(txt.Text))
                        txt.Text = string.Empty;
                }*/
            }
        }

        public Func<LogItem, bool> OnBeforeCheckFilter;

        public Func<LogItem, bool, bool> OnAfterCheckFilter;

        public bool IsFilteringEnabled { get; set; }

        #endregion

        #region Private Methods

        protected string getTextBoxName(string prop)
        {
            return string.Format("txtFilter{0}", prop).Replace(".", "");
        }

        protected bool itemCheckFilter(object obj)
        {
            if (!IsFilteringEnabled)
            {
                return true;
            }
            LogItem item = (LogItem)obj;

            if (OnBeforeCheckFilter != null)
            {
                if (!OnBeforeCheckFilter(item))
                {
                    return false;
                }
            }
            
            if (!Filter.Matches(item))
            {
                return false;
            }

            if (OnBeforeCheckFilter != null)
            {
                if (!OnBeforeCheckFilter(item))
                {
                    return false;
                }
            }

            return true;

            /*
            if (_filterPropertyList != null && _txtSearchPanel != null)
            {
                if (_markCheckBox == null)
                {
                    _markCheckBox = _txtSearchPanel.FindName("IsMarkedFilterName") as CheckBox;
                }

                if (_markCheckBox.IsChecked.HasValue)
                {
                    LogItem logItem = (LogItem)item;
                    if (logItem.IsMarked != _markCheckBox.IsChecked.Value)
                    {
                        return false;
                    }
                }

                //Check each string filter property
                foreach (string prop in _filterPropertyList)
                {
                    TextBox txt = null;
                    if (_txtCache.ContainsKey(prop))
                        txt = _txtCache[prop] as TextBox;
                    else
                    {
                        txt = _txtSearchPanel.FindName(getTextBoxName(prop)) as TextBox;
                        _txtCache[prop] = txt;
                    }

                    res = false;
                    if (txt == null)
                        res = true;
                    else
                    {
                        if (string.IsNullOrEmpty(txt.Text))
                            res = true;
                        else
                        {
                            try
                            {
                                //Get property value
                                object val = getItemValue(item, prop);
                                if (val != null)
                                {
                                    string valToCompare = string.Empty;
                                    if (val is DateTime)
                                        valToCompare = ((DateTime)val).ToString(GlobalHelper.DisplayDateTimeFormat, System.Globalization.CultureInfo.GetCultureInfo(Properties.Resources.CultureName));
                                    else
                                        valToCompare = val.ToString();

                                    if (valToCompare.ToString().IndexOf(txt.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                                        res = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                                res = true;
                            }
                        }
                    }
                    if (!res)
                        return res;
                }
            }
            res = true;
        }
        finally
        {
            if (OnAfterCheckFilter != null)
                res = OnAfterCheckFilter(item, res);

        }
        return res;
        */
        }

        protected object getItemValue(object item, string prop)
        {
            object val = null;
            try
            {
                val = item.GetType().GetProperty(prop).GetValue(item, null);
            }
            catch
            {
                val = null;
            }
            return val;
        }

        #endregion
    }

}