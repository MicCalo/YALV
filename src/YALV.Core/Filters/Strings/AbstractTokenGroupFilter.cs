using System.Text.RegularExpressions;

namespace YALV.Core.Filters.Strings
{
    public abstract class AbstractTokenGroupFilter : AbstractStringPropertyFilter<IFilterToken[]>
    {
        protected readonly bool _isAnd;
        protected AbstractTokenGroupFilter(bool isAnd, bool ignoreCase) : base(ignoreCase)
        {
            _isAnd = isAnd;
        }

        protected override bool Test(string given)
        {
            int pos = 0;
            foreach (IFilterToken token in filterValue)
            {
                bool isMatch = token.Matches(given, ref pos);
                if (isMatch && !_isAnd)
                {
                    return true;
                }

                if (!isMatch && _isAnd)
                {
                    return false;
                }
            }

            return _isAnd;
        }
    }
}
