using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace YALV.Core.Plugins.Formatting
{
    public class DocumentHelper
    {
        protected static Brush background = new SolidColorBrush(Colors.White);
        protected static Thickness padding = new Thickness(1);

        static DocumentHelper()
        {
            background.Freeze();
        }

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
            paragraph.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(paragraph);
            return doc;
        }

        protected virtual void AddContent(string text, Paragraph paragraph, FlowDocument doc)
        {
            paragraph.Inlines.Add(new Run(text));
        }

        protected void AddFormatted(string msg, Paragraph paragraph, MatchCollection matches, Func<Group, Match, FormatInfo> formatFunct)
        {
            int pos = 0;
            foreach (Match m in matches)
            {
                foreach (Group g in m.Groups)
                {
                    if (g.Index == m.Index && g.Length == m.Length)
                    {
                        continue;
                    }
                    if (g.Index <= pos)
                    {
                        continue;
                    }
                    if (pos < g.Index)
                    {
                        paragraph.Inlines.Add(FormatFactory.Create(msg.Substring(pos, g.Index - pos), FormatInfo.Normal));
                    }

                    FormatInfo info = formatFunct(g, m);
                    paragraph.Inlines.Add(FormatFactory.Create(msg.Substring(g.Index, g.Length), info));
                    pos = g.Index + g.Length;
                }
            }
            if (pos < msg.Length)
            {
                paragraph.Inlines.Add(FormatFactory.Create(msg.Substring(pos, msg.Length - pos), FormatInfo.Normal));
            }
        } 
    }
}
