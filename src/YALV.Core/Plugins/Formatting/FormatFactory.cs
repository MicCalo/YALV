using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace YALV.Core.Plugins.Formatting
{
    [Flags]
    public enum TextStyleFlags
    {
        None = 0,
        Bold = 0x000001,
        Italic = 0x000002,

        Hyperlink = 0x000080,
    }

    public enum FormatInfoParameters
    {
        TextStyleFlags,
        ForegroundColor,
        BackgroundColor,
        HyperLinkCommand,
        HyperLinkCommandParameter,
    }

    public class FormatInfo
    {
        public static readonly FormatInfo Normal = new FormatInfo();
        public static readonly FormatInfo Bold = new FormatInfo(TextStyleFlags.Bold);
        public static readonly FormatInfo Italics = new FormatInfo(TextStyleFlags.Italic);

        private readonly Dictionary<FormatInfoParameters, object> settings = new Dictionary<FormatInfoParameters, object>(3);

        public FormatInfo()
        {
        }

        public FormatInfo(ICommand hyperlinkCmd, object hyperlinkParam) : this(TextStyleFlags.Hyperlink)
        {
            settings.Add(FormatInfoParameters.HyperLinkCommand, hyperlinkCmd);
            settings.Add(FormatInfoParameters.HyperLinkCommandParameter, hyperlinkParam);
        }

        public FormatInfo(TextStyleFlags textStyle)
        {
            settings.Add(FormatInfoParameters.TextStyleFlags, textStyle);
        }

        public FormatInfo(TextStyleFlags textStyle, Brush foreground)
        {
            settings.Add(FormatInfoParameters.TextStyleFlags, textStyle);
            settings.Add(FormatInfoParameters.ForegroundColor, foreground);
        }

        public void Set(FormatInfoParameters param, object value)
        {
            settings[param] = value;
        }

        public object Get(FormatInfoParameters param)
        {
            object result;
            settings.TryGetValue(param, out result);
            return result;
        }

        public T Get<T>(FormatInfoParameters param)
        {
            object result;
            if (!settings.TryGetValue(param, out result))
            {
                return default(T);
            }
            return (T)result;
        }

        public bool TryGet<T>(FormatInfoParameters param, out T value)
        {
            object result;
            if (!settings.TryGetValue(param, out result))
            {
                value = default(T);
                return false;
            }
            value = (T)result;
            return true;
        }
    }

    public static class FormatFactory
    {
        public static Inline Create(string text, FormatInfo info)
        {
            Inline result = new Run(text);
            TextStyleFlags style;
            if (info.TryGet(FormatInfoParameters.TextStyleFlags, out style))
            {
                if (style.HasFlag(TextStyleFlags.Bold))
                {
                    result = new Bold(result);
                }

                if (style.HasFlag(TextStyleFlags.Italic))
                {
                    result = new Italic(result);
                }

                if (style.HasFlag(TextStyleFlags.Hyperlink))
                {
                    Hyperlink h = new Hyperlink(result);
                    ICommand cmd;
                    if (info.TryGet(FormatInfoParameters.HyperLinkCommand, out cmd))
                    {
                        h.Command = cmd;
                    }
                    h.CommandParameter = info.Get(FormatInfoParameters.HyperLinkCommandParameter);
                    result = h;
                }
            }

            Brush brush;
            if (info.TryGet(FormatInfoParameters.ForegroundColor, out brush))
            {
                result.Foreground = brush;
            }

            if (info.TryGet(FormatInfoParameters.BackgroundColor, out brush))
            {
                result.Background = brush;
            }

            return result;
        }
    }
}
