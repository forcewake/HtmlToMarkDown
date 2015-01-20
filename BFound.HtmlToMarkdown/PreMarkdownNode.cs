namespace BFound.HtmlToMarkdown
{
    using System;

    public class PreMarkdownNode : MarkDownNode
    {
        public override string ToString()
        {
            var code = base.ToString();

            var line = this.GetCodeHeader() + code + this.GetCodeFooter(code);

            return line;
        }

        private string GetCodeHeader()
        {
            return this.NewLine() + "```" + this.NewLine();
        }

        private string GetCodeFooter(string code)
        {
            var footer = "```" + this.NewLine();

            if (!code.EndsWith(Environment.NewLine))
            {
                footer = this.NewLine() + footer;
            }

            return footer;
        }
    }
}