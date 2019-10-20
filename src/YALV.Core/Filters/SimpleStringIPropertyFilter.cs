using System.Linq;
using YALV.Core.Domain;

namespace YALV.Core.Filters
{
    public class SimpleStringIPropertyFilter : IPropertyFilter
    {
        private readonly string filterValue;

        public SimpleStringIPropertyFilter(string val)
        {
            filterValue = val;
        }

        public bool Matches(LogItem item, LogItemProperty property)
        {
            object given = item.Get(property);
            if (given == null || given.Equals(string.Empty)){
                return true;
            }

            return given.ToString().Contains(filterValue);
        }

    }
}
