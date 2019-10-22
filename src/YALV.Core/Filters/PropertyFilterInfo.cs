using System.Windows.Controls;

namespace YALV.Core.Filters
{
    public class PropertyFilterInfo: IPropertyFilterInfo
    {
        public PropertyFilterInfo(Control control, IPropertyFilter filter)
        {
            Control = control;
            Filter = filter;
        }

        public Control Control { get; private set; }
        public IPropertyFilter Filter { get; private set; }
    }
}
