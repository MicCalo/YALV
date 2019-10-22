using System.Windows.Documents;
using YALV.Core.Domain;

namespace YALV.Core.Plugins.Formatting
{
    public class FormattingLogMessageDetailCreator : DocumentHelper, ILogMessageDetailCreatorPlugin
    {
        private static IYalvPluginInformation _info = new YalvPluginInformation("Normal Log Details", "Base and fallback for all log message details", "(c) 2019 Michel Calonder", new System.Version(1, 0, 0));
        
        public virtual int Priority { get { return int.MaxValue; } }

        public virtual bool IsEnabled
        {
            get { return true; }
        }

        public virtual IYalvPluginInformation Information
        {
            get { return _info; }
        }        

        public virtual FlowDocument GenerateDetailMessage(LogItem item)
        {
            Paragraph p;
            FlowDocument doc = CreateDocument(out p);
            AddContent(item.Message, p, doc);
            return doc;
        }

        public virtual bool IsSuitingForDetailMessage(LogItem item)
        {
            return true;
        }
    }
}
