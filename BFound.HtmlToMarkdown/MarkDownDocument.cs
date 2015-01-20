using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BFound.HtmlToMarkdown
{
    using BFound.HtmlToMarkdown.Extensions;

    using HtmlAgilityPack;

    public static class MarkDownDocument
    {
        private static Dictionary<string, Func<HtmlNode, MarkDownNode, MarkDownNode>> ElementConverters;

        static MarkDownDocument()
        {
            ElementConverters = new Dictionary<string,Func<HtmlNode,MarkDownNode,MarkDownNode>>{
                {"p", (htmlNode, markdownNode) => markdownNode.Append(new ParagraphMarkDownNode())},
                {"b", (htmlNode, markdownNode) => markdownNode.Append(new BoldMarkDownNode())},
                {"strong", (htmlNode, markdownNode) => markdownNode.Append(new BoldMarkDownNode())},
                {"i", (htmlNode, markdownNode) => markdownNode.Append(new ItalicMarkDownNode())},
                {"em", (htmlNode, markdownNode) => markdownNode.Append(new ItalicMarkDownNode())},
                {"a", (htmlNode, markdownNode) => markdownNode = markdownNode.Append(new LinkMarkDownNode { Href = AttrValue(htmlNode, "href") })},
                {"img", (htmlNode, markdownNode) => markdownNode.Append(new ImageMarkDownNode { Alt = AttrValue(htmlNode, "alt"), Src = AttrValue(htmlNode, "src") })},
                {"div", (htmlNode, markdownNode) => markdownNode.Append(new DivMarkDownNode())},
                {"br", (htmlNode, markdownNode) => markdownNode.Append(new BreakMarkDownNode())},
                {"tr", (htmlNode, markdownNode) => markdownNode.Append(new BreakMarkDownNode())},
                {"hr", (htmlNode, markdownNode) => markdownNode.Append(new HorizontalRuleMarkDownNode())},
                {"h1", (htmlNode, markdownNode) => markdownNode.Append(new HeadingMarkDownNode(1))},
                {"h2", (htmlNode, markdownNode) => markdownNode.Append(new HeadingMarkDownNode(2))},
                {"h3", (htmlNode, markdownNode) => markdownNode.Append(new HeadingMarkDownNode(3))},
                {"h4", (htmlNode, markdownNode) => markdownNode.Append(new HeadingMarkDownNode(4))},
                {"h5", (htmlNode, markdownNode) => markdownNode.Append(new HeadingMarkDownNode(5))},
                {"blockquote", (htmlNode, markdownNode) => markdownNode.Append(new BlockquoteMarkDownNode())},
                {"ul", (htmlNode, markdownNode) => markdownNode.Append(new UnorderdListMarkDownNode())},
                {"ol", (htmlNode, markdownNode) => markdownNode.Append(new OrderdListMarkDownNode())},
                {"li", (htmlNode, markdownNode) => markdownNode.Append(new ListItemMarkDownNode())},
                {"style", (htmlNode, markdownNode) => markdownNode.Append(new EmptyMarkdownNode())},
                {"script", (htmlNode, markdownNode) => markdownNode.Append(new EmptyMarkdownNode())},
                {"pre", (htmlNode, markdownNode) => markdownNode.Append(new PreMarkdownNode())},
                {"code", (htmlNode, markdownNode) =>
                    {
                        var parentNodeName = htmlNode.ParentNode.Name;
                        var code = WebUtility.HtmlDecode(htmlNode.InnerText);
                        MarkDownNode node = markdownNode.Append(parentNodeName.ToLowerInvariant().Equals("pre") ? new CodeMarkdownNode(code) : new InlineCodeMarkdownNode(code));
                        node.HasChildNodes = false;
                        return node;
                    } },
                {"table", (htmlNode, markdownNode) => markdownNode.Append(new TableMarkdownNode(htmlNode.ExtractTable()))},
                {"#text", TextToMarkDown},
            };
        }

        public static string FromHtml(string html)
        {
            return FromHtml(html, null);
        }

        public static string FromHtml(string html, Dictionary<string, Func<HtmlNode, MarkDownNode, MarkDownNode>> customElementConverters)
        {
            var elementConverters = ElementConverters;
            if (customElementConverters != null)
            {
                foreach (var conveter in customElementConverters)
                {
                    elementConverters[conveter.Key] = conveter.Value;
                }
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var body = htmlDocument.DocumentNode.SelectSingleNode("//body");

            var mardownDocumentNode = new MarkDownNode();
            HtmlNodeToMarkDownNode(body ?? htmlDocument.DocumentNode, mardownDocumentNode, elementConverters);

            return mardownDocumentNode.ToString();
        }

        private static MarkDownNode TextToMarkDown(HtmlNode htmlNode, MarkDownNode markdownNode)
        {
            var text = System.Text.RegularExpressions.Regex.Replace(htmlNode.InnerText, "\\s+", " ");
            return markdownNode.Append(new TextMarkDownNode { Text = WebUtility.HtmlDecode(text) });
        }

        private static void HtmlNodeToMarkDownNode(
            HtmlNode htmlNode,
            MarkDownNode markdownNode,
            Dictionary<string, Func<HtmlNode, MarkDownNode, MarkDownNode>> elementConverters)
        {
            Func<HtmlNode, MarkDownNode, MarkDownNode> elementConverter;
            
            if (elementConverters.TryGetValue(htmlNode.Name.ToLowerInvariant(), out elementConverter))
            {
                markdownNode = elementConverter(htmlNode, markdownNode);
            }

            if (markdownNode.HasChildNodes)
            {
                foreach (var childNode in htmlNode.ChildNodes)
                {
                    HtmlNodeToMarkDownNode(childNode, markdownNode, elementConverters);
                }
            }
        }


        private static string AttrValue(HtmlNode htmlNode, string name)
        {
            var attr = htmlNode.Attributes[name];
            return attr != null
                ? attr.Value
                : null;
        }
    }
}
