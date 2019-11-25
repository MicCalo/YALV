using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using YALV.Core.Domain;
using YALV.Core.Plugins;

namespace YALV.Core.Filters
{
    public interface IFilterManager :IYalvPlugin
    {
        IFilter CreateFilter();
        IPropertyFilter CreateFilterProperty(LogItemProperty prop, Control control);
        IPropertyFilterInfo CreateFilterInfo(LogItemProperty prop, Action<LogItemProperty, Control> changedAction);
    }
}
