using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPlora.Core.WebCrawler
{
    public interface IWebCrawler
    {
        Task<List<string>> RetrieveNewsLinks(string rootUri);

        Task<Tuple<string,string>> RetrieveTextFromHtmlPage(string webPageUrl);
    }
}
