namespace YALV.Core.Filters.Strings
{
    public class ContainsFullStringPropertyFilter : AbstractStringPropertyFilter<string>
    {
        public ContainsFullStringPropertyFilter(bool ignoreCase) : base(ignoreCase) { }

        protected override bool Test(string given)
        {
            return given.Contains(filterValue);
        }

        protected override string GetFilterValue(string input)
        {
            return input;
        }
    }
}
