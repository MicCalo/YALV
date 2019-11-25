using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;
using YALV.Core.Domain;

namespace YALV.Core.Plugins.Formatting
{
    public class FormattingDetailThrowableCreator : DocumentHelper, ILogThrowableDetailPlugin
    {
        private static IYalvPluginInformation _info = new YalvPluginInformation("Normal Throwable Details", "Base and fallback for all throwable details", "(c) 2019 Michel Calonder", new System.Version(1, 0, 0));
        
        public virtual int Priority { get { return int.MaxValue; } }

        public virtual bool IsEnabled
        {
            get { return true; }
        }

        public virtual IYalvPluginInformation Information
        {
            get { return _info; }
        }        

        public virtual FlowDocument GenerateThrowable(LogItem item)
        {
            Paragraph p;
            FlowDocument doc = CreateDocument(out p);
            AddContent(item.Throwable, p, doc);
            return doc;
        }

        public virtual bool IsSuitingForDetailThrowabe(LogItem item)
        {
            return true;
        }
    }
}
