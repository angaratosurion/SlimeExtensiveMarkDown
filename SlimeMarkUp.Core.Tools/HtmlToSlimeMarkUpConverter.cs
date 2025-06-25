using HtmlAgilityPack;
using System.Text;
using System.Linq;

namespace SlimeMarkUp.Tools
{
    public class ConverterSettings
    {
        public bool AddExtraNewLines { get; set; } = false;
        public string ListIndent { get; set; } = "  "; // Για nested λίστες
    }

    public class HtmlToSlimeMarkUpConverter
    {
        private readonly ConverterSettings _settings;

        public HtmlToSlimeMarkUpConverter(ConverterSettings? settings = null)
        {
            _settings = settings ?? new ConverterSettings();
        }

        public string Convert(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return ConvertNode(doc.DocumentNode, 0);
        }

        public void ConvertToFile(string html, string outputPath)
        {
            var slime = Convert(html);
            File.WriteAllText(outputPath, slime);
        }

        private string ConvertNode(HtmlNode node, int indentLevel)
        {
            var sb = new StringBuilder();
            foreach (var child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "h1": sb.AppendLine("# " + child.InnerText.Trim()); break;
                    case "h2": sb.AppendLine("## " + child.InnerText.Trim()); break;
                    case "h3": sb.AppendLine("### " + child.InnerText.Trim()); break;
                    case "h4": sb.AppendLine("#### " + child.InnerText.Trim()); break;
                    case "h5": sb.AppendLine("##### " + child.InnerText.Trim()); break;
                    case "h6": sb.AppendLine("###### " + child.InnerText.Trim()); break;
                    case "p": sb.AppendLine(ConvertNode(child, indentLevel).Trim()); break;

                    case "strong": sb.Append("**" + child.InnerText.Trim() + "**"); break;
                    case "em": sb.Append("*" + child.InnerText.Trim() + "*"); break;
                    case "s": sb.Append("~~" + child.InnerText.Trim() + "~~"); break;
                    case "u": sb.Append("++" + child.InnerText.Trim() + "++"); break;
                    case "code": sb.Append("`" + child.InnerText.Trim() + "`"); break;
                    case "br": sb.AppendLine(); break;
                    case "hr": sb.AppendLine("---"); break;
                    case "ul": sb.AppendLine(ConvertList(child, indentLevel)); break;
                    case "blockquote": sb.AppendLine(ConvertBlockquote(child)); break;
                    case "pre": sb.AppendLine(ConvertCode(child)); break;
                    case "table": sb.AppendLine(ConvertTable(child)); break;
                    case "img": sb.AppendLine(ConvertImage(child)); break;
                    case "a": sb.Append(ConvertLink(child)); break;
                    default:
                        if (child.HasChildNodes)
                            sb.Append(ConvertNode(child, indentLevel));
                        else
                            sb.Append(child.InnerText);
                        break;
                }

                if (_settings.AddExtraNewLines)
                    sb.AppendLine();
            }
            return sb.ToString();
        }

        private string ConvertList(HtmlNode ul, int level)
        {
            var sb = new StringBuilder();
            foreach (var li in ul.SelectNodes("li") ?? Enumerable.Empty<HtmlNode>())
            {
                var indent = string.Concat(Enumerable.Repeat(_settings.ListIndent, level));
                sb.Append(indent + "- ");
                sb.AppendLine(li.InnerText.Trim());

                // nested ul inside li
                var nestedUl = li.SelectSingleNode("ul");
                if (nestedUl != null)
                    sb.Append(ConvertList(nestedUl, level + 1));
            }
            return sb.ToString();
        }

        private string ConvertBlockquote(HtmlNode node)
        {
            var lines = node.InnerText.Split('\n').Select(l => "> " + l.Trim());
            return string.Join("\n", lines) + "\n";
        }

        private string ConvertCode(HtmlNode node)
        {
            return "```\n" + node.InnerText.Trim() + "\n```\n";
        }

        private string ConvertTable(HtmlNode table)
        {
            var sb = new StringBuilder();
            var rows = table.SelectNodes("tr") ?? Enumerable.Empty<HtmlNode>();
            var rowList = rows.ToList();
            if (rowList.Count == 0) return "";

            var headers = rowList[0].SelectNodes("td|th").Select(cell => cell.InnerText.Trim()).ToList();
            sb.AppendLine("| " + string.Join(" | ", headers) + " |");
            sb.AppendLine("|" + string.Join("|", headers.Select(h => new string('-', h.Length + 2))) + "|");

            for (int i = 1; i < rowList.Count; i++)
            {
                var cells = rowList[i].SelectNodes("td|th").Select(cell => cell.InnerText.Trim());
                sb.AppendLine("| " + string.Join(" | ", cells) + " |");
            }

            return sb.ToString();
        }

        private string ConvertImage(HtmlNode node)
        {
            var alt = node.GetAttributeValue("alt", "");
            var src = node.GetAttributeValue("src", "");
            var width = node.GetAttributeValue("width", null);
            var height = node.GetAttributeValue("height", null);
            var props = new List<string>();
            if (width != null) props.Add($"width={width}");
            if (height != null) props.Add($"height={height}");
            var propStr = props.Count > 0 ? "{" + string.Join(" ", props) + "}" : "";
            return $"![{alt}]({src}){propStr}\n";
        }

        private string ConvertLink(HtmlNode node)
        {
            var href = node.GetAttributeValue("href", "#");
            var target = node.GetAttributeValue("target", null);
            var rel = node.GetAttributeValue("rel", null);
            var text = node.InnerText.Trim();
            var props = new List<string>();
            if (target != null) props.Add($"target={target}");
            if (rel != null) props.Add($"rel={rel}");
            var propStr = props.Count > 0 ? "{" + string.Join(" ", props) + "}" : "";
            return $"[{text}]({href}){propStr}";
        }
    }
}
