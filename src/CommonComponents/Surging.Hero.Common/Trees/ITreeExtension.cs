using Surging.Hero.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Surging.Core.Domain
{
    public static class ITreeExtension
    {
        public static IEnumerable<ITree<T>> BuildTree<T>(this IEnumerable<ITree<T>> treeData) where T : class {
            var treeResult = new List<ITree<T>>();
            var topNodes = treeData.Where(p => p.ParentId == 0);
            foreach (var topNode in topNodes) {
                topNode.FullName = topNode.Name;
                topNode.Children = GetTreeChildren(topNode,treeData);
                treeResult.Add(topNode);
            }
            return treeResult;
        }

        private static IEnumerable<ITree<T>> GetTreeChildren<T>(ITree<T> node, IEnumerable<ITree<T>> treeData) where T : class
        {
            var children = treeData.Where(p=>p.ParentId == node.Id);
            if (children.Any()) {
                foreach (var child in children)
                {
                    child.FullName = node.FullName + HeroConstants.CodeRuleRestrain.FullNameSeparator + child.Name;
                    child.Children = GetTreeChildren(child, treeData);
                }
            }
            return children;

        }
    }
}
