using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using YALV.Core.Domain;

namespace YALV.Core.Filters
{
    public class SimpleBoolPropertyFilter : IPropertyFilter
    {
        private bool? _expectation;

        public bool Matches(LogItem item, LogItemProperty property)
        {
            if (!_expectation.HasValue)
            {
                return true;
            }

            return _expectation.Value.Equals(item.Get(property));
        }

        public void Update(Control source)
        {
            CheckBox cb = source as CheckBox;
            if (cb != null)
            {
                _expectation = cb.IsChecked;
            }
        }
    }
}
