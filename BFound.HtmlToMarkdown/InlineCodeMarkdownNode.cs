namespace BFound.HtmlToMarkdown
{
    public class InlineCodeMarkdownNode : CodeMarkdownNode
    {
        public InlineCodeMarkdownNode(string codeBlock)
            : base(codeBlock)
        {
        }

        public override string ToString()
        {
            return " `" + this.CodeBlock + "` ";
        }
    }
}