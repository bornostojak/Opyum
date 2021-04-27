using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Opyum.WindowsPlatform
{
    public static class XmlToString
    {
        public static string ReturnXMLAsString(XmlNode node)
        {
            XmlWriterSettings xws = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                NewLineOnAttributes = true,
                Indent = true,
                Encoding = Encoding.Default
            };
            using (var strw = new StringWriter()) using (var xtw = XmlWriter.Create(strw, xws))
            {
                node.WriteTo(xtw);
                xtw.Flush();
                return strw.GetStringBuilder().ToString();
            }
        }
    }
}
