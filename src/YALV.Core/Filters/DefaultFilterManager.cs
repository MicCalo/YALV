﻿using System;
using System.Windows;
using System.Windows.Controls;
using YALV.Core.Domain;
using YALV.Core.Filters.Strings;
using YALV.Core.Plugins;

namespace YALV.Core.Filters
{
    public class DefaultFilterManager : IFilterManager
    {
        private static IYalvPluginInformation _info = new YalvPluginInformation("Filtering support", "Basic implemention for filtering", "(c) 2019 Michel Calonder", new Version(1, 0, 0));

        public virtual int Priority
        {
            get { return int.MaxValue; }
        }

        public bool IsEnabled
        {
            get { return true; }
        }

        public IYalvPluginInformation Information { get { return _info; } }

        public IPropertyFilterInfo CreateFilterInfo(LogItemProperty prop, Action<LogItemProperty, Control> changedAction)
        {
            Control ctrl;
            if (prop == LogItemProperty.IsMarked)
            {
                CheckBox cb = new CheckBox();
                cb.IsThreeState = true;
                cb.IsChecked = null;
                cb.Click += Cb_Click;
                ctrl = cb;
            }
            else
            {
                TextBox tb = new TextBox();
                Style txtStyle = Application.Current.FindResource("RoundWatermarkTextBox") as Style;
                if (txtStyle != null)
                {
                    tb.Style = txtStyle;
                }
                tb.KeyUp += Tb_KeyUp;
                ctrl = tb;
            }

            IPropertyFilter filter = CreateFilterProperty(prop, ctrl);
            ctrl.DataContext = new Holder(changedAction, prop, filter);
            return new PropertyFilterInfo(ctrl, filter);
        }

        public IFilter CreateFilter()
        {
            return new DefaultFilter();
        }

        public IPropertyFilter CreateFilterProperty(LogItemProperty prop, Control control)
        {
            IPropertyFilter result;
            if (prop == LogItemProperty.IsMarked)
            {
                result = new SimpleBoolPropertyFilter();
            }
            else
            {
                result = new ContainsAllTokenStringPropertyFilter(true, true);
            }

            result.Update(control);
            return result;
        }

        private void Call(object sender)
        {
            Control ctrl = (Control)sender;
            ((Holder)ctrl.DataContext).Call(ctrl);
        }

        private void Cb_Click(object sender, RoutedEventArgs e)
        {
            Call(sender);
        }

        private void Tb_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Call(sender);
        }

        struct Holder
        {
            private Action<LogItemProperty, Control> _changedAction;
            private LogItemProperty _property;
            private IPropertyFilter _filter;

            public Holder(Action<LogItemProperty, Control> changedAction, LogItemProperty property, IPropertyFilter filter)
            {
                _changedAction = changedAction;
                _property = property;
                _filter = filter;
            }

            public void Call(Control ctrl)
            {
                _filter.Update(ctrl);
                _changedAction(_property, ctrl);
            }
        }
    }
}
