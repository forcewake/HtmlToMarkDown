namespace BFound.HtmlToMarkdown
{
    public class PreMarkdownNode : MarkDownNode
    {
        public override string ToString()
        {
            var line = this.NewLine() + "```" + this.NewLine() + base.ToString() + this.NewLine() + "```" + this.NewLine();
            return line;
        }
    }
}