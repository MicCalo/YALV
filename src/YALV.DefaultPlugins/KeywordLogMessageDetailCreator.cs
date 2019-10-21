using System.Text.RegularExpressions;
using System.Windows.Documents;
using YALV.Core.Domain;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Formatting;

namespace YALV.DefaultPlugins
{
    public class KeywordLogMessageDetailCreator : FormattingLogMessageDetailCreator
    {
        private static IYalvPluginInformation _info = new YalvPluginInformation("Highligjted keywords Log Details", "Highlights some keywords in the log detail (work in progress)", "(c) 2019 Michel Calonder", new System.Version(0, 9, 0));

        private readonly Regex regex = new Regex("([Dd]ebug)");

        public override int Priority { get { return int.MaxValue - 200; } }

        public override bool IsEnabled
        {
            get { return true; }
        }

        public override IYalvPluginInformation Information {get{return _info;} }

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
