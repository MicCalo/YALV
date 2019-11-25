namespace YALV.Core.Filters.Strings
{
    public interface IFilterToken
    {
        bool Matches(string given, ref int currentPosition);
    }
}
