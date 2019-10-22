using System.Linq;
using System.Windows.Controls;
using YALV.Core.Domain;

namespace YALV.Core.Filters
{
    public class SimpleStringIPropertyFilter : IPropertyFilter
    {
        private string filterValue;

        public SimpleStringIPropertyFilter(string val)
        {
            filterValue = val;
        }

        public bool Matches(LogItem item, LogItemProperty property)
        {
            if (string.IsNullOrEmpty(filterValue))
            {
                return true;
            }
            object given = item.Get(property);
            if (given == null || given.Equals(string.Empty)){
                return true;
            }

            return given.ToString().Contains(filterValue);
        }

        public void Update(Control source)
        {
            TextBox tb = source as TextBox;
            if (tb != null)
            {
                filterValue = tb.Text;
            }
        }
    }
}
