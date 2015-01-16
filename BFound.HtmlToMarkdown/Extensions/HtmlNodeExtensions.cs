namespace BFound.HtmlToMarkdown.Extensions
{
    using System.Linq;

    using HtmlAgilityPack;

    using MarkdownLog;

    public static class HtmlNodeExtensions
    {
        public static Table ExtractTable(this HtmlNode htmlNode)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(htmlNode.InnerHtml);

            var headers = doc.DocumentNode.SelectNodes("//tr/th");

            var dataRows =
                doc.DocumentNode.SelectNodes("//tr[td]")
                    .Select(row => row.SelectNodes("td").Select(td => td.InnerText).ToArray())
                    .ToList()
                    .AsReadOnly();

            var newColumns =
                headers.Select(header => new TableColumn { HeaderCell = new TableCell { Text = header.InnerText } })
                    .ToList();

            var rows = dataRows.ToTableRows();

            return new Table { Columns = newColumns, Rows = rows };
        }
    }
}