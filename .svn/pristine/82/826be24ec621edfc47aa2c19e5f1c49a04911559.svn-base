using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing.Helpers
{
    public static class HtmlNodeHelpers
    {
        public static IEnumerable<HtmlNode> GetNextSiblings(this HtmlNode node)
        {
            for (var sibling = node.NextSibling; sibling != null; sibling = sibling.NextSibling)
            {
                yield return sibling;
            }
        }

        public static IEnumerable<HtmlNode> GetPreviousSiblings(this HtmlNode node)
        {
            for (var sibling = node.PreviousSibling; sibling != null; sibling = sibling.PreviousSibling)
            {
                yield return sibling;
            }
        }

        public static IEnumerable<HtmlNode> GetNextSiblings(this HtmlNode node, string name)
        {
            return node.GetNextSiblings().Where(sibling => sibling.Name == name);
        }

        public static string GetTextOfTextChildren(this HtmlNode node)
        {
            var builder = new StringBuilder();
            foreach (var child in node.ChildNodes.Where(ch => ch.NodeType == HtmlNodeType.Text))
            {
                builder.Append(child.InnerText);
            }
            return builder.ToString();
        }

        public static IEnumerable<HtmlNode> WhereAttribute (this IEnumerable<HtmlNode> nodes, string attributeName, Predicate<string> valueFilter)
        {
            return nodes.Where(node => node.Attributes.Contains(attributeName) && valueFilter(node.Attributes[attributeName].Value));
        }

        public static IEnumerable<HtmlNode> WhereAttributeEquals(this IEnumerable<HtmlNode> nodes, string attributeName, string attributeValue)
        {
            return nodes.WhereAttribute(attributeName, @value => @value.Equals(attributeValue));
        }

        public static IEnumerable<HtmlNode> WhereIdEquals(this IEnumerable<HtmlNode> nodes, string idValue)
        {
            return nodes.WhereAttributeEquals("id", idValue);
        }

        public static IEnumerable<HtmlNode> WhereClassEquals(this IEnumerable<HtmlNode> nodes, string classValue)
        {
            return nodes.WhereAttributeEquals("class", classValue);
        }

        public static IEnumerable<HtmlNode> WhereClassContains(this IEnumerable<HtmlNode> nodes, string classValue)
        {
            return nodes.WhereAttribute("class", @class => @class.Split(' ').Contains(classValue));
        }

        /// <summary>
        /// Checks if "id" attribute of <see cref="htmlNode"/> equals <see cref="idAttributeValue"/>
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="idAttributeValue"></param>
        /// <returns></returns>
        public static bool IdEquals(this HtmlNode htmlNode, string idAttributeValue)
        {
            return htmlNode.AttributeEquals("id", idAttributeValue);
        }

        /// <summary>
        /// Checks if <see cref="attributeName"/> attribute of <see cref="htmlNode"/> equals <see cref="attributeValue"/>
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="attributeName"> </param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public static bool AttributeEquals(this HtmlNode htmlNode, string attributeName, string attributeValue)
        {
            return htmlNode.Attributes.Contains(attributeName) && htmlNode.Attributes [attributeName].Value.EndsWith(attributeValue);
        }

        public static void RemoveComments (this HtmlNode parentNode)
        {
            var nodes = parentNode.SelectNodes("//comment()");
            if (nodes != null)
            {
                foreach (HtmlNode comment in nodes)
                {
                    comment.ParentNode.RemoveChild(comment);
                }
            }
        }
    }
}