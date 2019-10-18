using YALV.Core.Domain;

namespace YALV.Filters
{
    public interface IPropertyFilter
    {
        bool Matches(LogItem item, LogItemProperty property);
    }
}
