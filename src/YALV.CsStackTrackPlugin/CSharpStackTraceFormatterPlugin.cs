using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Runtime.InteropServices;
using System.Windows.Input;
using log4net;
using YALV.Core.Domain;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Formatting;

namespace YALV.CsStackTrackPlugin
{
    // https://www.helixoft.com/blog/creating-envdte-dte-for-vs-2017-from-outside-of-the-devenv-exe.html
    public class CSharpStackTraceFormatterPlugin : FormattingDetailThrowableCreator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CSharpStackTraceFormatterPlugin));
        private static readonly Regex Regex = new Regex(@"\s*(?:at|bei) [\w`<>\.]*\.(?<method>[\w\[\]<>_`]*)\([\[\]\w ,&`]*\)(?: in (?<file>\w:[\w\\.\-_]*):(?:line|Zeile) (?<line>\d*))?");

        private static IYalvPluginInformation _info = new YalvPluginInformation("C# stack trace highlighting", "Throwable display with C# stack trace highlighting", "(c) 2019 Michel Calonder", new Version(1, 0, 0));

        private readonly ICommand _hyperlinkCommand;

        public CSharpStackTraceFormatterPlugin(IPluginContext pluginContext)
        {
            _hyperlinkCommand = new Command(pluginContext.PluginDirectory);
        }

        public override int Priority
        {
            get { return 100; }
        }

        public override IYalvPluginInformation Information
        {
            get { return _info; }
        }

        protected override void AddContent(string msg, Paragraph paragraph, FlowDocument doc)
        {
            MatchCollection mc = Regex.Matches(msg);
            AddFormatted(msg, paragraph, mc, GetFormat);
        }

        private FormatInfo GetFormat(Group group, Match match)
        {
            string name = group.Name;
            if ("method".Equals(name))
            {
                return FormatInfo.Bold;
            }
            if ("file".Equals(name))
            {
                string lineNumberString = match.Groups["line"].Value;
                return new FormatInfo(_hyperlinkCommand, new string[] { group.Value, lineNumberString });
            }
            return FormatInfo.Normal;
        }

        public override bool IsSuitingForDetailThrowabe(LogItem item)
        {
            if (string.IsNullOrEmpty(item.Throwable))
            {
                return false;
            }

            bool isMatch = Regex.IsMatch(item.Throwable);
            return isMatch;
        }

        class Command : ICommand
        {
           // private readonly DirectoryInfo _pluginDir;
            private readonly string _toolPath;
            public Command(DirectoryInfo pluginDir)
            {
                _toolPath = Path.Combine(pluginDir.FullName, "VisualStudioFileOpenTool.exe");
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                int studioVersion = 17;
                string vsString = GetVersionString(studioVersion);
                try
                {
                    string[] parameters = (string[]) parameter;
                    string filename = parameters[0];
                    int fileline = int.Parse(parameters[1]);
                    EnvDTE80.DTE2 dte2;
                    dte2 = (EnvDTE80.DTE2) Marshal.GetActiveObject(vsString);
                    dte2.MainWindow.Activate();
                    EnvDTE.Window w = dte2.ItemOperations.OpenFile(filename, EnvDTE.Constants.vsViewKindTextView);
                    ((EnvDTE.TextSelection) dte2.ActiveDocument.Selection).GotoLine(fileline, true);
                }
                catch (Exception ex)
                {
                    Log.Warn(string.Format("Couldn't open Visual Studio via DTE. Version assumed to be {0} -> DTE string is {1} ", studioVersion, vsString), ex);
                }
            }

            private static string GetVersionString(int visualStudioVersionNumber)
            {
                //  Source: http://www.mztools.com/articles/2011/MZ2011011.aspx
                switch (visualStudioVersionNumber)
                {
                    case 17:
                        return "VisualStudio.DTE.15.0";
                    case 15:
                        return "VisualStudio.DTE.14.0";
                    case 13:
                        return "VisualStudio.DTE.12.0";
                    case 12:
                        return "VisualStudio.DTE.11.0";
                    case 10:
                        return "VisualStudio.DTE.10.0";
                    case 8:
                        return "VisualStudio.DTE.9.0";
                    case 5:
                        return "VisualStudio.DTE.8.0";
                    case 3:
                        return "VisualStudio.DTE.7.1";
                    case 2:
                        return "VisualStudio.DTE.7";
                }

                throw new NotSupportedException("visualStudioVersionNumber " + visualStudioVersionNumber);
            }
        }
    }
}
