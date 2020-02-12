using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using log4net;

namespace YALV.Core.Settings
{
    internal class ConfigIo
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ConfigIo));

        private FileInfo GetFile()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return new FileInfo( Path.Combine(path, "YALV.settings.xml"));
        }

        public bool TryLoad(Dictionary<string, object> settings)
        {
            FileInfo f = GetFile();
            if (!f.Exists)
            {
                return false;
            }

            using (Stream s = f.OpenRead())
            {
                XmlReader rdr = XmlReader.Create(s);
                while (rdr.Read())
                {
                    if (rdr.NodeType == XmlNodeType.Element && "item".Equals(rdr.Name))
                    {
                        string key = rdr.GetAttribute("key");
                        string type = rdr.GetAttribute("type");
                        object o = Read(rdr, type);
                        settings.Add(key, o);
                    }
                }
            }

            return true;
        }

        private object Read(XmlReader rdr, string type)
        {
            if (type.EndsWith("[]"))
            {
                List<object> result = new List<object>();
                string entryType = type.Substring(0, type.Length - 2);
                Type t = GetType(entryType);
                bool isDone = false;
                while (rdr.Read() && !isDone)
                {
                    if (rdr.NodeType == XmlNodeType.Element)
                    {
                        result.Add(Read(rdr, entryType));
                    }
                    else if (rdr.NodeType == XmlNodeType.EndElement)
                    {
                        if (rdr.Name.Equals("item"))
                        {
                            isDone = true;
                        }
                    }
                }

                Array a = Array.CreateInstance(t, result.Count);
                for (int i = 0; i < result.Count; i++)
                {
                    a.SetValue(result[i], i);
                }
                return a;
            }
            else
            {
                switch (type)
                {
                    case ("String"):
                        return WebUtility.HtmlDecode(rdr.ReadElementContentAsString());
                    case ("Regex"):
                        {
                            return new Regex(WebUtility.HtmlDecode(rdr.ReadElementContentAsString()));
                        }
                    default: throw new NotSupportedException("Type " + type);
                }
            }
        }

        private Type GetType(string t)
        {
            switch (t)
            {
                case ("String"): return typeof(string);
                default: throw new NotSupportedException("Type " + t);
            }
        }

        public void Save(Dictionary<string, object> settings)
        {
            try
            {
                using (XmlWriter w = XmlWriter.Create(new StreamWriter(GetFile().OpenWrite()),
                    new XmlWriterSettings() {Indent = true}))
                {
                    w.WriteStartElement("settings");
                    foreach (KeyValuePair<string, object> kv in settings)
                    {
                        w.WriteStartElement("item");
                        w.WriteAttributeString("key", kv.Key);
                        w.WriteAttributeString("type", kv.Value.GetType().Name);
                        Write(w, kv.Value);
                        w.WriteEndElement();
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                log.InfoFormat("Can not write file '{0}' because of missing permission", GetFile().FullName);
            }
        }

        private void Write(XmlWriter w, object o)
        {
            Type t = o.GetType();
            if (t.IsArray)
            {
                Array a = (Array)o;
                foreach (object s in a)
                {
                    w.WriteStartElement("entry");
                    Write(w, s);
                    w.WriteEndElement();
                }
            }
            else
            {
                string s = WebUtility.HtmlEncode(o.ToString());
                w.WriteString(s);
            }
        }
    }
}
