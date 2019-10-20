using YALV.Core.Domain;

namespace YALV.Core.Filters
{
    public interface IPropertyFilter
    {
        bool Matches(LogItem item, LogItemProperty property);
    }
}
