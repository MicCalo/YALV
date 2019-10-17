using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YALV.Core.Domain;

namespace YALV.Common
{
    public interface IPropertyFilter
    {
        bool Matches(LogItem item, LogItemProperty property);
    }

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

    public class FilterInfo
    {
        private Dictionary<LogItemProperty, IPropertyFilter> filters = new Dictionary<LogItemProperty, IPropertyFilter>();

        public void Add(LogItemProperty prop, IPropertyFilter f)
    }
}
