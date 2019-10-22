using System.Text.RegularExpressions;
using System.Windows.Documents;
using YALV.Core.Domain;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Formatting;

namespace YALV.DefaultPlugins
{
    public class KeywordLogMessageDetailCreator : FormattingLogMessageDetailCreator
    {
        private static IYalvPluginInformation _info = new YalvPluginInformation("Highlighted keywords Log Details", "Highlights some keywords in the log detail (work in progress)", "(c) 2019 Michel Calonder", new System.Version(0, 9, 0));

        private readonly Regex regex = new Regex("([Dd]ebug)");

        public override int Priority { get { return int.MaxValue - 200; } }

        public override bool IsEnabled
        {
            get { return true; }
        }

        public override IYalvPluginInformation Information {get{return _info;} }

        protected override void AddContent(string msg, Paragraph paragraph, FlowDocument doc)
        {
            MatchCollection mc = regex.Matches(msg);
            AddFormatted(msg, paragraph, mc, (grp, match) => FormatInfo.Bold);
        }

        public override bool IsSuitingForDetailMessage(LogItem item)
        {
            return regex.IsMatch(item.Message);
        }
    }
}
