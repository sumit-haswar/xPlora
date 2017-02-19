using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using xPlora.Core.WebCrawler;
using System.IO;

namespace xPlora.GoogleNewsCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Corpus generation in progress...");
                Processor processor = new Processor();
                IWebCrawler webCrawler = new WebCrawler();

                var linksFilePath = ConfigurationManager.AppSettings.GetValues("GoogleNewsLinkFileName");
                

                //construct file name
                var currDirectory = Directory.GetCurrentDirectory();

                //get all urls from file
                List<Tuple<string, string>> urlList = processor.GetPageUrls(currDirectory + @"\" + linksFilePath.FirstOrDefault());

                //asynchronously invoke retrieve page-data for each link             
                List<Task<Tuple<string, string>>> taskList = new List<Task<Tuple<string, string>>>();

                for (int i = 0; i < urlList.Count; i++){
                    int idx = i;
                    Task<Tuple<string, string>> t = Task<Tuple<string, string>>.Factory.StartNew(() => webCrawler.RetrieveTextFromHtmlPage(urlList[idx].Item2).Result);
                    taskList.Add(t);
                }

                Task.WaitAll(taskList.ToArray());   //wait for all tasks

                int fileCount = 0;
                foreach (var task in taskList) {
                    try{
                        fileCount++;
                        processor.CreateTextFile(task.Result.Item1, task.Result.Item2, "file" + fileCount.ToString());
                    }
                    catch {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
