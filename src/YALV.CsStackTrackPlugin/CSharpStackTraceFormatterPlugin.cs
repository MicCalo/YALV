using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using YALV.Core.Domain;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Formatting;

namespace YALV.CsStackTrackPlugin
{
    public class CSharpStackTraceFormatterPlugin : FormattingDetailThrowableCreator
    {
        //https://stackoverflow.com/questions/21785363/programatically-open-file-in-visual-studio
        //C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE
        //https://stackoverflow.com/questions/350323/open-a-file-in-visual-studio-at-a-specific-line-number

       // private static readonly Regex regex = new Regex(@"^\s*at [\w\.]*\.(?<method>\w*)\([\[\]\w ,&`]*\) in (?<file>\w:[\w\\.\-_]*):line (?<line>\d*)?$");
        private static readonly Regex regex = new Regex(@"\s*(?:at|bei) [\w\.]*\.(?<method>\w*)\([\[\]\w ,&`]*\)(?: in (?<file>\w:[\w\\.\-_]*):(?:line|Zeile) (?<line>\d*))?");

        private static IYalvPluginInformation _info = new YalvPluginInformation("C# stack trace highlighting", "Throwable dispaly with C# stack trace highlighting", "(c) 2019 Michel Calonder", new Version(1, 0, 0));

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
            MatchCollection mc = regex.Matches(msg);
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

            bool isMatch = regex.IsMatch(item.Throwable);
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
                string[] parameters = (string[])parameter;
                string filename = parameters[0];
                int fileline = int.Parse(parameters[1]);
                string vsString = GetVersionString(15);
                EnvDTE80.DTE2 dte2;
                dte2 = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject(vsString);
                dte2.MainWindow.Activate();
                EnvDTE.Window w = dte2.ItemOperations.OpenFile(filename, EnvDTE.Constants.vsViewKindTextView);
                ((EnvDTE.TextSelection)dte2.ActiveDocument.Selection).GotoLine(fileline, true);


                /*
                string args = string.Format("{0} \"{1}\" {2}", 17, parameters[0], parameters[1]);
                Process.Start(_toolPath, args);
                Debug.WriteLine("Open hyperlink "+parameter);
                */
            }

            private static string GetVersionString(int visualStudioVersionNumber)
            {
                //  Source: http://www.mztools.com/articles/2011/MZ2011011.aspx
                switch (visualStudioVersionNumber)
                {
                    case 17:
                        return "VisualStudio.DTE.16.0";
                    case 15:
                        return "VisualStudio.DTE.15.0";
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
