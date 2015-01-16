namespace BFound.HtmlToMarkdown
{
    public class CodeMarkdownNode : MarkDownNode
    {
        public CodeMarkdownNode(string codeBlock)
        {
            this.CodeBlock = codeBlock;
        }

        public string CodeBlock { get; private set; }

        public override string ToString()
        {
            return this.CodeBlock;
        }
    }
}