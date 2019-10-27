using System.Windows.Controls;
using YALV.Core.Domain;

namespace YALV.Core.Filters.Strings
{
    public abstract class AbstractStringPropertyFilter<T> : IPropertyFilter
    {
        protected T filterValue;
        protected bool ignoreCase;

        protected AbstractStringPropertyFilter(bool ignoreCase)
        {
            this.ignoreCase = ignoreCase;
        }

        protected abstract T GetFilterValue(string input);
        protected abstract bool Test(string given);

        public bool Matches(LogItem item, LogItemProperty property)
        {
            if (filterValue ==null)
            {
                return true;
            }

            object given = item.Get(property);
            if (given == null || given.Equals(string.Empty))
            {
                return true;
            }
            string g = given.ToString();
            if (ignoreCase)
            {
                g = g.ToLowerInvariant();
            }
            return Test(g);
        }

        public void Update(Control source)
        {
            TextBox tb = source as TextBox;
            if (tb != null)
            {
                Update(tb.Text);
            }
        }

        public void Update(string value)
        {
            if (ignoreCase)
            {
                value = value.ToLowerInvariant();
            }

            filterValue = GetFilterValue(value);
        }
    }
}
