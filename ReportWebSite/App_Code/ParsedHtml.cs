using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

public sealed class ParsedHtml
{
    private class Entry
    {
        public string Text { get; set; }
        public HtmlNode Node { get; set; }
        public int Offset { get; set; }
    }

    private readonly List<Entry> _buffer = new List<Entry>();
    private string _output;

    public HtmlDocument Document { get; set; }

    public void Add(string text, HtmlNode node)
    {
        _buffer.Add(new Entry() { Text = text, Node = node });
    }

    public void AppendNewline()
    {
        if (_buffer.Count > 0)
            _buffer.Last().Text += "\n";
    }

    public void Process(string html)
    {
        this.Document = new HtmlDocument();
        this.Document.LoadHtml(html);

        foreach (HtmlNode node in EnumAll(this.Document))
        {
            if (node.NodeType == HtmlNodeType.Text)
            {
                string nodeText = ((HtmlTextNode)node).Text;
                nodeText = nodeText.Replace("&nbsp;", " ");
                nodeText = nodeText.Replace("&quot;", "\"");
                nodeText = nodeText.Replace("&lt;", "<");
                nodeText = nodeText.Replace("&gt;", ">");
                nodeText = nodeText.Replace("&iexcl;", "¡");
                nodeText = nodeText.Replace("&cent;", "¢");
                nodeText = nodeText.Replace("&pound;", "£");
                nodeText = nodeText.Replace("&curren;", "¤");
                nodeText = nodeText.Replace("&yen;", "¥");
                nodeText = nodeText.Replace("&brvbar;", "¦");
                nodeText = nodeText.Replace("&sect;", "§");
                nodeText = nodeText.Replace("&uml;", "¨");
                nodeText = nodeText.Replace("&copy;", "©");
                nodeText = nodeText.Replace("&ordf;", "ª");
                nodeText = nodeText.Replace("&laquo;", "«");
                nodeText = nodeText.Replace("&not;", "¬");
                nodeText = nodeText.Replace("&shy;", "­-");
                nodeText = nodeText.Replace("&reg;", "®");
                nodeText = nodeText.Replace("&macr;", "¯");
                nodeText = nodeText.Replace("&deg;", "°");
                nodeText = nodeText.Replace("&plusmn;", "±");
                nodeText = nodeText.Replace("&sup2", "²");
                nodeText = nodeText.Replace("&sup3;", "³");
                nodeText = nodeText.Replace("&acute;", "´");
                nodeText = nodeText.Replace("&micro;", "µ");
                nodeText = nodeText.Replace("&para;", "¶");
                nodeText = nodeText.Replace("&middot;", "·");
                nodeText = nodeText.Replace("&cedil;", "¸");
                nodeText = nodeText.Replace("&sup1;", "¹");
                nodeText = nodeText.Replace("&ordm;", "º");
                nodeText = nodeText.Replace("&raquo;", "»");
                nodeText = nodeText.Replace("&frac14;", "¼");
                nodeText = nodeText.Replace("&frac12;", "½");
                nodeText = nodeText.Replace("&frac34;", "¾");
                nodeText = nodeText.Replace("&iquest;", "¿");
                nodeText = nodeText.Replace("&times;", "×");
                nodeText = nodeText.Replace("&divide;", "÷");
                nodeText = nodeText.Replace("&ETH;", "Ð");
                nodeText = nodeText.Replace("&eth;", "ð");
                nodeText = nodeText.Replace("&THORN;", "Þ");
                nodeText = nodeText.Replace("&thorn;", "þ");
                nodeText = nodeText.Replace("&AElig;", "Æ");
                nodeText = nodeText.Replace("&aelig;", "æ");
                nodeText = nodeText.Replace("&OElig;", "Œ");
                nodeText = nodeText.Replace("&oelig;", "œ");
                nodeText = nodeText.Replace("&Aring;", "Å");
                nodeText = nodeText.Replace("&Oslash;", "Ø");
                nodeText = nodeText.Replace("&Ccedil;", "Ç");
                nodeText = nodeText.Replace("&ccedil;", "ç");
                nodeText = nodeText.Replace("&szlig;", "ß");
                nodeText = nodeText.Replace("&Ntilde;", "Ñ");
                nodeText = nodeText.Replace("&ntilde;", "ñ");
                nodeText = nodeText.Replace("&ldquo;", "“");
                nodeText = nodeText.Replace("&rdquo;", "”");
                foreach (Match match in Regex.Matches(nodeText, @"&#(?<value>\d+);").Cast<Match>().Reverse())
                {
                    nodeText = nodeText.Remove(match.Index, match.Length);
                    char ch = ' ';
                    int value;
                    if (int.TryParse(match.Groups["value"].Value, out value))
                        ch = (char)value;
                    nodeText = nodeText.Insert(match.Index, new string(ch, 1));
                }
                nodeText = nodeText.Replace("&amp;", "&");

                this.Add(nodeText, node);

                if (node.NextSibling == null)
                {
                    HtmlNode parent = node.ParentNode;
                    while (parent != null
                        && !parent.Name.Equals("div", StringComparison.OrdinalIgnoreCase)
                        && !parent.Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                    {
                        if (parent.NextSibling != null)
                        {
                            parent = null;
                            break;
                        }
                        parent = parent.ParentNode;
                    }
                    if (parent != null)
                    {
                        this.AppendNewline();
                    }
                }
            }
            else if (node.NodeType == HtmlNodeType.Element)
            {
                if (node.Name.Equals("br", StringComparison.OrdinalIgnoreCase))
                {
                    this.AppendNewline();
                }
                else if (node.ChildNodes.Count == 0)
                {
                    if (node.NextSibling == null)
                    {
                        HtmlNode parent = node.ParentNode;
                        while (parent != null
                            && !parent.Name.Equals("div", StringComparison.OrdinalIgnoreCase)
                            && !parent.Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                        {
                            if (parent.NextSibling != null)
                            {
                                parent = null;
                                break;
                            }
                            parent = parent.ParentNode;
                        }
                        if (parent != null)
                        {
                            this.AppendNewline();
                        }
                    }
                }
            }
        }

        StringBuilder output = new StringBuilder();
        foreach (Entry entry in _buffer)
        {
            entry.Offset = output.Length;
            output.Append(entry.Text);
        }
        _output = output.ToString().TrimEnd();
    }

    public void Truncate(int length)
    {
        if (length >= _output.Length)
            return;

        foreach (Entry entry in _buffer.Where(x => x.Offset >= length))
        {
            HtmlNode node = entry.Node;
            HtmlNode parentNode = node.ParentNode;

            if (parentNode == null)
                continue;

            // Destroy THIS node, and whatever siblings following this node
            for (HtmlNode sibling = node.NextSibling;
                node != null;
                node = sibling, sibling = (sibling == null ? null : sibling.NextSibling))
            {
                parentNode.RemoveChild(node);
            }

            // Prune empty parents as well
            while (parentNode.ParentNode != null && parentNode.ChildNodes.Count == 0)
            {
                node = parentNode;
                parentNode = parentNode.ParentNode;
                parentNode.RemoveChild(node);
            }
        }

        HtmlNode lastGoodNode = _buffer.Where(x => x.Offset < length).Select(x => x.Node).LastOrDefault();
        while (lastGoodNode != null)
        {
            // Destroy all nodes beyond the last last node known to contribute text
            for (HtmlNode sibling = lastGoodNode.NextSibling;
                sibling != null;
                sibling = lastGoodNode.NextSibling)
            {
                lastGoodNode.ParentNode.RemoveChild(sibling);
            }

            // Repeat for the whole DOM hierarchy
            lastGoodNode = lastGoodNode.ParentNode;
        }
    }

    public override string ToString()
    {
        return _output;
    }

    static IEnumerable<HtmlNode> EnumAll(HtmlDocument doc)
    {
        return EnumAll(doc.DocumentNode);
    }

    static IEnumerable<HtmlNode> EnumAll(HtmlNode node)
    {
        if (node.NodeType == HtmlNodeType.Comment)
            yield break;
        if (node.NodeType == HtmlNodeType.Element && node.Name.Equals("head", StringComparison.OrdinalIgnoreCase))
            yield break;

        yield return node;

        foreach (HtmlNode child in node.ChildNodes)
        {
            foreach (HtmlNode result in EnumAll(child))
                yield return result;
        }
    }
}
