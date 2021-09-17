using System.Collections.Generic;
using System.Collections.Specialized;

namespace WebScraping
{
    public class DirectoryTreeModel
    {
        public string RootURL { get; set; }
        public List<DirectoryNode> Nodes { get; set; } = new List<DirectoryNode>();

        public List<DirectoryNode> GetLeafNodes()
        {
            var output = new List<DirectoryNode>();

            foreach (var node in Nodes)
                AddLeafFromParent(node,output);

            return output;
        }

        public string GetUrlFromHref(string href)
        {
            return RootURL.Replace("categories", href);
        }

        public List<DirectoryNode> RootFromLeaf(DirectoryNode leaf)
        {
            //TODO
            return null;
        }
        

        private void AddLeafFromParent(DirectoryNode parent, List<DirectoryNode> nodes)
        {
            if (!parent.HasChildren)
            {
                nodes.Add(parent);
                return;
            }

            foreach (var child in parent.Children)
            {
                child.Parent = parent;
                AddLeafFromParent(child, nodes);
            }
        }
    }
}