using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xPlora.Core.Util;

namespace xPlora.Core.WebCrawler
{
    public class WebCrawler : IWebCrawler
    {

        public async Task<List<string>> RetrieveNewsLinks(string rootUri)
        {
            List<string> newsUriList = new List<string>();

            Uri rootRequestURI = new Uri(rootUri);

            HttpClient httpClient = new HttpClient();
            var rootResponsePage = await httpClient.GetByteArrayAsync(rootRequestURI);
            String rootPageSource = Encoding.GetEncoding("utf-8").GetString(rootResponsePage);

            rootPageSource = WebUtility.HtmlDecode(rootPageSource);

            HtmlDocument rootHtmlDocument = new HtmlDocument();

            rootHtmlDocument.LoadHtml(rootPageSource);

            string divLeadArticle = "esc-lead-article-title";
            string divSecondaryArticle = "esc-secondary-article-title-wrapper";
            string divDiversityArticle = "esc-diversity-article-wrapper";

            List<string> listArticle = new List<string>() { divLeadArticle, divSecondaryArticle, divDiversityArticle };

            var articleListDiv
                = rootHtmlDocument.DocumentNode.Descendants().Where(d => d.Attributes.Contains("class")
                    && listArticle.Contains(d.Attributes["class"].Value));

            if (articleListDiv != null && articleListDiv.Count() > 0)
            {
                foreach (var newsArticleItem in articleListDiv)
                {
                    string newsUrl = RegexUtil.ExtractHref(newsArticleItem.InnerHtml.Replace('"', '\''));
                    newsUriList.Add(newsUrl);
                }
            }

            return newsUriList;
        }

        public async Task<Tuple<string,string>> RetrieveTextFromHtmlPage(string webPageUrl)
        {
            try
            {
                string plainText = string.Empty;
                Uri webPageUri = null;
                if (Uri.TryCreate(webPageUrl, UriKind.Absolute, out webPageUri))
                {
                    if (webPageUri.Scheme == "http")
                    {
                        HttpClient httpClient = new HttpClient();
                        var webPage = await httpClient.GetByteArrayAsync(webPageUri);

                        string webPageSource = Encoding.GetEncoding("utf-8").GetString(webPage);
                        webPageSource = WebUtility.HtmlDecode(webPageSource);

                        //Regular Expression to Remove Script Tags from Html Content
                        var regexScript = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

                        string htmlText = regexScript.Replace(webPageSource, "");

                        Regex regexHtml = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);

                        string resultText = regexHtml.Replace(htmlText, " ").Trim();
                        plainText = resultText.ToString();
                    }
                }
                return new Tuple<string,string>(webPageUrl, plainText);
            }
            catch (HttpRequestException)
            {
                return new Tuple<string, string>(string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
