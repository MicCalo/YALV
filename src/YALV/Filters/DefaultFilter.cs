using System.Collections.Generic;
using YALV.Core.Domain;

namespace YALV.Filters
{
    public class DefaultFilter : IFilter
    {
        private Dictionary<LogItemProperty, IPropertyFilter> _filters = new Dictionary<LogItemProperty, IPropertyFilter>();

        public void Add(LogItemProperty prop, IPropertyFilter filter)
        {
            if (filter == null)
            {
                _filters.Remove(prop);
            }
            else
            {
                _filters[prop] = filter;
            }
        } 

        public void Remove(LogItemProperty prop)
        {
            _filters.Remove(prop);
        }

        public bool Matches(LogItem item)
        {
            foreach(KeyValuePair<LogItemProperty, IPropertyFilter> kv in _filters)
            {
                if (!kv.Value.Matches(item, kv.Key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
