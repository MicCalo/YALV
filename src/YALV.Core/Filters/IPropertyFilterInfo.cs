using System.Windows.Controls;

namespace YALV.Core.Filters
{
    public interface IPropertyFilterInfo
    {
        Control Control { get; }
        IPropertyFilter Filter { get; }
    }
}
