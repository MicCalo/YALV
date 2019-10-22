using System.Windows.Documents;
using YALV.Core.Domain;

namespace YALV.Core.Plugins.Formatting
{
    public interface ILogThrowableDetailPlugin : IYalvPlugin
    {
        bool IsSuitingForDetailThrowabe(LogItem item);
        FlowDocument GenerateThrowable(LogItem item);
    }
}
