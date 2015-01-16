namespace BFound.HtmlToMarkdown.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using MarkdownLog;

    public static class EnumerableExtensions
    {
        public static IEnumerable<TableRow> ToTableRows(this IReadOnlyCollection<string[]> source)
        {
            var columnCount = source.First().Length;

            return source.Select(
                row =>
                new TableRow
                    {
                        Cells =
                            Enumerable.Range(0, columnCount)
                            .Select(i => new TableCell { Text = row[i].ToString() })
                    }).ToList();
        } 
    }
}