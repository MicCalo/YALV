using System.Windows.Controls;
using YALV.Core.Domain;

namespace YALV.Core.Filters
{
    public interface IPropertyFilter
    {
        void Update(Control source);
        bool Matches(LogItem item, LogItemProperty property);
    }
}
