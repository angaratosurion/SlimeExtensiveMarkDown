using System.Collections.Generic;
using System.Text;

namespace SlimeMarkUp.Core
{
    public class HtmlExporter
    {
        public string Export(List<MarkupElement> elements)
        {
            var sb = new StringBuilder();
            foreach (var element in elements)
            {
                ExportElement(element, sb);
            }
            return sb.ToString();
        }

        private void ExportElement(MarkupElement element, StringBuilder sb)
        {
            sb.Append('<').Append(element.Tag);

            if (element.Attributes != null)
            {
                foreach (var attr in element.Attributes)
                {
                    sb.Append(' ').Append(attr.Key).Append("=\"").Append(attr.Value).Append('"');
                }
            }

            sb.Append('>');

            if (!string.IsNullOrEmpty(element.Content))
            {
                sb.Append(element.Content);
            }

            if (element.Children != null)
            {
                foreach (var child in element.Children)
                {
                    ExportElement(child, sb);
                }
            }

            sb.Append("</").Append(element.Tag).Append('>');
        }
    }
}
