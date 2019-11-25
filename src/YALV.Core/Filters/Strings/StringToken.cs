namespace YALV.Core.Filters.Strings
{
    public class StringToken : IFilterToken
    {
        private readonly string expected;

        public StringToken(string expected)
        {
            this.expected = expected;
        }

        public bool Matches(string given, ref int currentPos)
        {
            int i = given.IndexOf(expected, currentPos);
            currentPos = i + expected.Length;
            return i > -1;
        }
    }
}
