using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using YALV.Core.Domain;

namespace YALV.Core.Plugins.Formatting
{
    public class FormattingLogMessageDetailCreator : ILogMessageDetailCreatorPlugin
    {
        protected static Brush background = new SolidColorBrush(Colors.White);
        protected static Thickness padding = new Thickness(1);

        static FormattingLogMessageDetailCreator()
        {
            background.Freeze();
        }

        public virtual int Priority { get { return int.MaxValue; } }

        protected virtual FlowDocument CreateDocument(out Paragraph paragraph)
        {
            FlowDocument doc = new FlowDocument();
            doc.Background = background;
            doc.PagePadding = padding;
            doc.FontSize = SystemFonts.MessageFontSize;
            doc.FontFamily = SystemFonts.MessageFontFamily;
            doc.FontWeight = SystemFonts.MessageFontWeight;
            doc.FontStyle = SystemFonts.MessageFontStyle;
            paragraph = new Paragraph();
            doc.Blocks.Add(paragraph);
            return doc;
        }

        protected virtual void AddContent(LogItem item, Paragraph  firstParagraph, FlowDocument doc)
        {
            firstParagraph.Inlines.Add(new Run(item.Message));
        }

        public virtual FlowDocument GenerateDetailMessage(LogItem item)
        {
            Paragraph p;
            FlowDocument doc = CreateDocument(out p);
            AddContent(item, p, doc);
            return doc;
        }

        public virtual bool IsSuitingForDetailMessage(LogItem item)
        {
            return true;
        }
    }
}
