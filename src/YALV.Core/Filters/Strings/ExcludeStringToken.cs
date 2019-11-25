using System.Linq;

namespace YALV.Core.Filters.Strings
{
    public class ExcludeStringToken : IFilterToken
    {
        private readonly string notExpected;

        public ExcludeStringToken(string notExpected)
        {
            this.notExpected = notExpected;
        }

        public bool Matches(string given, ref int currentPos)
        {
            return !given.Contains(notExpected);
        }
    }
}
