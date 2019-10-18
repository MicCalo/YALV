using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YALV.Core.Domain;

namespace YALV.Filters
{
    public class SimpleBoolPropertyFilter : IPropertyFilter
    {
        private bool? _expectation;

        public SimpleBoolPropertyFilter(bool? expectation)
        {
            _expectation = expectation;
        }

        public bool Matches(LogItem item, LogItemProperty property)
        {
            if (!_expectation.HasValue)
            {
                return true;
            }

            return _expectation.Value.Equals(item.Get(property));
        }
    }
}
