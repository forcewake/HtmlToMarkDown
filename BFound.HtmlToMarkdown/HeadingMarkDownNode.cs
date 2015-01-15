using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFound.HtmlToMarkdown
{
    public class HeadingMarkDownNode : MarkDownNode
    {
        private int _level;

        public HeadingMarkDownNode(int level)
        {
            _level = level;
        }

        public override string ToString()
        {
            return NewLine() + new string('#', _level) + " " + base.ToString().TrimStart() + NewLine() + NewLine();
        }
    }
}
