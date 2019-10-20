using YALV.Core.Domain;

namespace YALV.Core.Filters
{
    public interface IFilter
    {
        void Add(LogItemProperty prop, IPropertyFilter filter);
        void Remove(LogItemProperty prop);
        bool Matches(LogItem item);
    }
}
