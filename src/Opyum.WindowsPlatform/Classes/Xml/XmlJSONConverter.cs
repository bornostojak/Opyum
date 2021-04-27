using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace Opyum.WindowsPlatform
{
    public static class XmlJSONConverter
    {
        public static XmlDocument ConvertJSONFileToXml(string json_file)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (File.Exists(json_file))
            {
                using (var file = File.Open(json_file, FileMode.Open))
                using (StreamReader sr = new StreamReader(file, Encoding.Default))
                {
                    stringBuilder.Append(sr.ReadToEnd());
                }
            }
            

            return JsonConvert.DeserializeXmlNode(stringBuilder.ToString());
        }

        
    }
}
