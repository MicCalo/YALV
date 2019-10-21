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
                }
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