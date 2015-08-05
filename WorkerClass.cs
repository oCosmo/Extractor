using HtmlAgilityPack;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Extractor
{
    class WorkerClass
    {
        public static string getSourceCode(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string sourceCode = sr.ReadToEnd();
            sr.Close();
            resp.Close();
            return sourceCode;
        }
        /// <summary>
        /// 
        /// teste
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Info getInfo(string url)
        {
            // The HtmlWeb class is a utility class to get the HTML over HTTP
            HtmlWeb htmlWeb = new HtmlWeb();

            // Creates an HtmlDocument object from an URL
            HtmlAgilityPack.HtmlDocument document = htmlWeb.Load(url);
            var siteInfo = new Info();
            siteInfo.Title = getTitle(document);

            siteInfo.Description = getDescription(document);

            siteInfo.tagH1 = getDescendantsTitles(document, "h1");

            siteInfo.tagH2 = getDescendantsTitles(document, "h2");

            siteInfo.tagH3 = getDescendantsTitles(document, "h3");

            return siteInfo;
        }

        public static string getContent(HtmlNode node)
        {
            if (node != null)
            {
                string content = node.Attributes["content"].Value;
                if (content.Trim() == "")
                {
                    return null;
                }
                else
                {
                    return content;
                }
            }
            else
            {
                return null;
            }
        }

        public static string getInerText(HtmlNode node)
        {
            if (node != null)
            {
                string content = node.InnerText;
                if (content.Trim() == "")
                {
                    return null;
                }
                else
                {
                    return content;
                }
            }
            else
            {
                return null;
            }
        }

        public static string getTitle(HtmlAgilityPack.HtmlDocument document)
        {
            string title = null;

            // First rule
            if (string.IsNullOrEmpty(title))
            {
                var someNode = document.DocumentNode.SelectSingleNode("//meta[@name='title']");
                if (someNode != null)
                {
                    title = getContent(someNode);
                }
            }
            if (string.IsNullOrEmpty(title))
            {
                var someNode = document.DocumentNode.SelectSingleNode("//title");
                if (someNode != null)
                {
                    title = getInerText(someNode);
                }
            }
            if (string.IsNullOrEmpty(title))
            {
                var someNode = document.DocumentNode.SelectSingleNode("//h1");
                if (someNode != null)
                {
                    title = getInerText(someNode);
                }
            }
            if (string.IsNullOrEmpty(title))
            {
                var someNode = document.DocumentNode.SelectSingleNode("//h2");
                if (someNode != null)
                {
                    title = getInerText(someNode);
                }
            }
            if (string.IsNullOrEmpty(title))
            {
                var someNode = document.DocumentNode.SelectSingleNode("//h3");
                if (someNode != null)
                {
                    title = getInerText(someNode);
                }
            }


            return title;

        }

        public static string getDescription(HtmlAgilityPack.HtmlDocument document)
        {
            string description = null;

            // First rule
            if (string.IsNullOrEmpty(description))
            {
                var someNode = document.DocumentNode.SelectSingleNode("//meta[@name='description']");
                if (someNode != null)
                {
                    description = getContent(someNode);
                }
            }
            if (string.IsNullOrEmpty(description))
            {
                var someNode = document.DocumentNode.SelectSingleNode("//description");
                if (someNode != null)
                {
                    description = getInerText(someNode);
                }
            }

            return description;

        }

        public static ArrayList getDescendantsTitles(HtmlAgilityPack.HtmlDocument document, string tag)
        {
            ArrayList valueAttribute = new ArrayList();

            foreach (HtmlAgilityPack.HtmlNode node in document.DocumentNode.Descendants(tag))
            {
                if (getInerText(node) != null || getInerText(node) != "")
                {
                    var verify = removeTag(getInerText(node));

                    if (verify != null || verify != "")
                    {
                        valueAttribute.Add(verify);
                    }
                }
            }
            return valueAttribute;
        }

        public static string removeTag(string valueAttribute)
        {
            var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);

            return reg.Replace(valueAttribute, "");
        }

        
        /// <summary>
        /// retorna alguns valores das tags informadas
        /// </summary>
        /// <param name="document">documento html a ser analizado</param>
        /// <param name="tags">tag que deve ser procurada no documento html</param>
        /// <returns>string com as informações da tag</returns>
        public static string getTags(HtmlAgilityPack.HtmlDocument document, string tags)
        {
            string result = "";

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes(tags);

            if (nodes == null)
                return result;

            foreach (HtmlNode node in nodes)
            {
                foreach (HtmlAttribute atributo in node.Attributes)
                {
                    result += "node: " + atributo.OwnerNode.Name + "   nome: " + atributo.Name + "     valor: " + atributo.Value;
                    result += Environment.NewLine;
                }
            }

            return result;
        }
    }
}
