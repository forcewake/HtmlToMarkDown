namespace BFound.HtmlToMarkdown
{
    using MarkdownLog;

    public class TableMarkdownNode : MarkDownNode
    {
        public TableMarkdownNode(Table table)
        {
            this.Table = table;
        }

        public Table Table { get; private set; }

        public override string ToString()
        {
            return this.NewLine() + this.Table.ToMarkdown() + this.NewLine();
        }
    }
}