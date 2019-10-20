using System;
using System.Collections.Generic;
using System.Linq;
using YALV.Core.Plugins;
using YALV.Core.Plugins.Formatting;

namespace YALV.Core.Domain
{
    public enum LogItemProperty
    {
        IsMarked,
        Id,
        Path,
        TimeStamp,
        Logger,
        Thread,
        Message,
        MachineName,
        UserName,
        HostName,
        App,
        Throwable,
        Class,
        Method,
        File,
        Line,
        Uncategorized,
        Level,
        LevelIndex
    }

    [Serializable]
    public class LogItem
    {
        private bool isMarked;
        public bool IsMarked { get { return isMarked; } set { isMarked = value; } }
        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime TimeStamp { get; set; }
        //public string Delta { get; set; }
        public double? Delta { get; set; }
        public string Logger { get; set; }
        public string Thread { get; set; }
        public string Message { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public string App { get; set; }
        public string Throwable { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string File { get; set; }
        public string Line { get; set; }
        public string Uncategorized { get; set; }

        /// <summary>
        /// LevelIndex
        /// </summary>
        public LevelIndex LevelIndex { get; set; }

        public object DetailMessage
        {
            get
            {
                IReadOnlyList<ILogMessageDetailCreatorPlugin> formatterPlugins = PluginManager.Instance.GetPlugins<ILogMessageDetailCreatorPlugin>();
                ILogMessageDetailCreatorPlugin formatter = formatterPlugins.FirstOrDefault(x => x.IsSuitingForDetailMessage(this));
                return formatter.GenerateDetailMessage(this);
            }
        }

        /// <summary>
        /// Level Property
        /// </summary>
        public string Level
        {
            get { return _level; }
            set
            {
                if (value != _level)
                {
                    _level = value;
                    assignLevelIndex(_level);
                }
            }
        }

        public object Get(LogItemProperty prop)
        {
            switch (prop)
            {
                case LogItemProperty.IsMarked: return IsMarked;
                case LogItemProperty.Id: return Id;
                case LogItemProperty.Path: return Path;
                case LogItemProperty.TimeStamp: return TimeStamp;
                case LogItemProperty.Logger: return Logger;
                case LogItemProperty.Thread: return Thread;
                case LogItemProperty.Message: return Message;
                case LogItemProperty.MachineName: return MachineName;
                case LogItemProperty.UserName: return UserName;
                case LogItemProperty.HostName: return HostName;
                case LogItemProperty.App: return App;
                case LogItemProperty.Throwable: return Throwable;
                case LogItemProperty.Class: return Class;
                case LogItemProperty.Method: return Method;
                case LogItemProperty.File: return File;
                case LogItemProperty.Line: return Line;
                case LogItemProperty.Uncategorized: return Uncategorized;
                case LogItemProperty.LevelIndex: return LevelIndex;
                default: throw new NotImplementedException("Property " + prop);
            }
        }
    

        #region Privates

        private string _level;

        private void assignLevelIndex(string level)
        {
            string ul = !String.IsNullOrWhiteSpace(level) ? level.Trim().ToUpper() : string.Empty;
            switch (ul)
            {
                case "DEBUG":
                    LevelIndex = LevelIndex.DEBUG;
                    break;
                case "INFO":
                    LevelIndex =  LevelIndex.INFO;
                    break;
                case "WARN":
                    LevelIndex =  LevelIndex.WARN;
                    break;
                case "ERROR":
                    LevelIndex =  LevelIndex.ERROR;
                    break;
                case "FATAL":
                    LevelIndex = LevelIndex.FATAL;
                    break;
                case "TRACE":
                    LevelIndex = LevelIndex.TRACE;
                    break;
                default:
                    LevelIndex =  LevelIndex.NONE;
                    break;
            }
        }

        #endregion
    }

    public enum LevelIndex
    {
        NONE = 0,
        DEBUG = 1,
        INFO = 2,
        WARN = 3,
        ERROR = 4,
        FATAL = 5,
        TRACE = 6
    }
}
