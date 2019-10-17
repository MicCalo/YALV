using System.Text.RegularExpressions;
using System.Windows.Documents;
using YALV.Core.Domain;
using YALV.Core.Plugins.Formatting;

namespace YALV.DefaultPlugins
{
    public class KeywordLogMessageDetailCreator : FormattingLogMessageDetailCreator
    {
        private readonly Regex regex = new Regex("([Dd]ebug)");

        public override int Priority { get { return int.MaxValue - 200; } }

        //https://stackoverflow.com/questions/21785363/programatically-open-file-in-visual-studio
        //C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE
        //https://stackoverflow.com/questions/350323/open-a-file-in-visual-studio-at-a-specific-line-number
        protected override void AddContent(LogItem item, Paragraph firstParagraph, FlowDocument doc)
        {
            string msg = item.Message;
            int pos = 0;
            MatchCollection mc = regex.Matches(msg);
            foreach (Match m in mc)
            {
                foreach (Group g in m.Groups)
                {
                    if (g.Index == 0 && g.Length == msg.Length)
                    {
                        continue;
                    }
                    if (g.Index <= pos){
                        continue;
                    }
                    if (pos < g.Index)
                    {
                        firstParagraph.Inlines.Add(new Run(msg.Substring(pos, g.Index - pos)));
                    }
                    firstParagraph.Inlines.Add(new Bold(new Run(msg.Substring(g.Index, g.Length))));
                    pos = g.Index + g.Length;
                }
            }
            if (pos < msg.Length)
            {
                firstParagraph.Inlines.Add(new Run(msg.Substring(pos, msg.Length - pos)));
            }
        }

        public override bool IsSuitingForDetailMessage(LogItem item)
        {
            return regex.IsMatch(item.Message);
        }
    }
}
