using System;
using System.ComponentModel;
using System.Reflection;

namespace YALV.ThreadViewPlugin.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged(string prop)
        {
            PropertyInfo p = this.GetType().GetProperty(prop);
            if (p == null)
            {
                throw new ArgumentException("Property '" + prop + "' is not a property of " + GetType().Name);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
