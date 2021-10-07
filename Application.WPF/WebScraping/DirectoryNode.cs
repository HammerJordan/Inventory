using System.Collections.Generic;

namespace Application.WPF.WebScraping
{
    public class DirectoryNode
    {
        public string Category { get; set; }
        public string Url { get; set; }
        public List<DirectoryNode> Children { get; set; } = new List<DirectoryNode>();

        public DirectoryNode Parent { get; set; }

        public DirectoryNode GetRootNode()
        {
            var currentNode = this;
            
            while (currentNode.Parent != null)
                currentNode = currentNode.Parent;

            return currentNode;
        }

        public DirectoryNode[] GetBranchNodesFromParent()
        {
            var output = new List<DirectoryNode>();
            var currentNode = this;

            while (currentNode.Parent != null)
            {
                output.Insert(0,currentNode);
                currentNode = currentNode.Parent;
            }

            return output.ToArray();
        }

        public bool HasChildren => Children.Count > 0;
    }
}