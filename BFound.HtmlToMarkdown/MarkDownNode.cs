using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFound.HtmlToMarkdown
{
    public class MarkDownNode
    {
        public MarkDownNode()
        {
            ChildNodes = new List<MarkDownNode>();
            HasChildNodes = true;
        }

        public MarkDownNode Parent { get; set; }

        public List<MarkDownNode> ChildNodes { get; private set; }

        public MarkDownNode Append(MarkDownNode node)
        {
            node.Parent = this;
            ChildNodes.Add(node);
            return node;
        }

        public virtual string NewLine()
        {
            if (Parent != null)
            {
                return Parent.NewLine();
            }
            return "\r\n";
        }

        public override string ToString()
        {
            return string.Join("", ChildNodes);
        }

        public bool HasChildNodes { get; set; }
    }
}
