using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using YALV.Core.Domain;
using YALV.Core.Plugins;

namespace YALV.Filters
{
    public interface IFilterManager :IYalvPlugin
    {
        IFilter CreateFilter();
        IPropertyFilter CreeateFilterProperty(LogItemProperty prop, Control control);
        Control CreateControl(LogItemProperty prop, Action<LogItemProperty, Control> changedAction);
    }

    public class DefaultFilterManager : IFilterManager
    {
        public virtual int Priority
        {
            get { return int.MaxValue; }
        }

        public Control CreateControl(LogItemProperty prop, Action<LogItemProperty, Control> changedAction)
        {
            if (prop == LogItemProperty.IsMarked)
            {
                CheckBox cb = new CheckBox();
                cb.IsThreeState = true;
                cb.IsChecked = true;
                cb.Click += Cb_Click;
                cb.Tag = new Holder(changedAction, prop);
                return cb;
            }

            TextBox tb = new TextBox();
            //tb.AcceptsReturn = false;
            Style txtStyle = Application.Current.FindResource("RoundWatermarkTextBox") as Style;
            if (txtStyle != null)
                tb.Style = txtStyle;
            tb.KeyUp += Tb_KeyUp;
            tb.Tag = new Holder(changedAction, prop);
            return tb;
        }

        public IFilter CreateFilter()
        {
            return new DefaultFilter();
        }

        public IPropertyFilter CreeateFilterProperty(LogItemProperty prop, Control control)
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
            ((Holder)ctrl.Tag).Call(ctrl);
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

            public Holder(Action<LogItemProperty, Control> changedAction, LogItemProperty property)
            {
                _changedAction = changedAction;
                _property = property;
            }

            public void Call(Control ctrl)
            {
                _changedAction(_property, ctrl);
            }
        }
    }
}
