using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xPlora.GoogleNewsCrawler
{
    public class Processor
    {

        public List<Tuple<string, string>> GetPageUrls(string xmlFilePath)
        {
            if (File.Exists(xmlFilePath))
            {
                XElement elem = XElement.Load(xmlFilePath);
                return (from e in elem.Elements("link")
                       select new Tuple<string, string>(e.Attribute("key") != null ? e.Attribute("key").Value : string.Empty, e.Value)).ToList();
            }
            return null;
        }

        public void CreateTextFile(string newsArticleUri, string content, string filePath)
        {
            if (string.IsNullOrWhiteSpace(newsArticleUri) == false && string.IsNullOrWhiteSpace(content) == false 
                && string.IsNullOrEmpty(filePath) == false)
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath.FirstOrDefault().ToString() + @"\" + filePath))
                {
                    streamWriter.WriteLine(newsArticleUri);
                    streamWriter.Write(content);
                }
            }
        }

    }
}
