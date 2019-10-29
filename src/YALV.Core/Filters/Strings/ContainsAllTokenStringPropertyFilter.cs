using System.Linq;
using System.Text.RegularExpressions;

namespace YALV.Core.Filters.Strings
{
    public class ContainsAllTokenStringPropertyFilter : AbstractTokenGroupFilter
    {
        private static readonly Regex negativeTokenRegex = new Regex(@"^-\w*$");
        private readonly bool _withExlusion;

        public ContainsAllTokenStringPropertyFilter(bool withExclusion, bool ignoreCase) : base(true, ignoreCase) {
            this._withExlusion = withExclusion;
        }

        protected override IFilterToken[] GetFilterValue(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new IFilterToken[0];
            }
            if (_withExlusion)
            {
                input = input.TrimEnd('-');
            }
            input = input.Trim(' ');
            string[] tokens = input.Split(' ');
            return tokens.Select(x => GetToken(x)).ToArray();
        }

        private IFilterToken GetToken(string token)
        {
            if (_withExlusion && negativeTokenRegex.IsMatch(token))
            {
                return new ExcludeStringToken(token.Substring(1));
            }

            return new StringToken(token);
        }
    }
}
