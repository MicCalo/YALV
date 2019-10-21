using System;
using System.Windows;
using System.Windows.Controls;
using YALV.Core.Domain;

namespace YALV.Core.Filters
{
    public class DefaultFilterManager : IFilterManager
    {
        public virtual int Priority
        {
            get { return int.MaxValue; }
        }

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
                //tb.AcceptsReturn = false;
                Style txtStyle = Application.Current.FindResource("RoundWatermarkTextBox") as Style;
                if (txtStyle != null)
                    tb.Style = txtStyle;
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
            if (prop == LogItemProperty.IsMarked)
            {
                return new SimpleBoolPropertyFilter(((CheckBox)control).IsChecked);
            }

            return new SimpleStringIPropertyFilter(((TextBox)control).Text);
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
