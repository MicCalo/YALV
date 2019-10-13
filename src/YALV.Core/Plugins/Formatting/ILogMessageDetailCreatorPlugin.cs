using System.Windows.Documents;
using YALV.Core.Domain;

namespace YALV.Core.Plugins.Formatting
{
    public interface ILogMessageDetailCreatorPlugin : IYalvPlugin
    {
        bool IsSuitingForDetailMessage(LogItem item);
        FlowDocument GenerateDetailMessage(LogItem item);
    }
}
