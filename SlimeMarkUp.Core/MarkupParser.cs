using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;

using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core
{
    public class MarkupParser
    {
        private readonly List<IBlockMarkupExtension> _extensions;

        public MarkupParser(IEnumerable<IBlockMarkupExtension> extensions)
        {
            _extensions = extensions.OrderBy(x => x.Order).ToList();
        }


        public List<MarkupElement> Parse(string text)
        {
            // �������� YAML ��� �� input
            string markupOnly = Regex.Replace(text, 
                @"^\s*---\s*\r?\n(.*?)\r?\n\s*---\s*", "",
                RegexOptions.Singleline);
            text = markupOnly;

            var elements = new List<MarkupElement>();
            var lines = new Queue<string>(text.Split('\n').Select(l => l.TrimEnd()));

            while (lines.Count > 0)
            {
                var line = lines.Peek();

                if (string.IsNullOrWhiteSpace(line))
                {
                    lines.Dequeue();
                    continue;
                }

                // ������� �������� YAML block �� �������� ��� document_properties:
                if (line.StartsWith("document_properties:"))
                {
                    lines.Dequeue(); // �������� �� document_properties:

                    // ������� ���� ��� ������� �� indent (���� � tab) � ����� �� ���������� �� �������
                    while (lines.Count > 0)
                    {
                        var nextLine = lines.Peek();

                        // �� � ������ ����� ���� � ��� ���� ������ indent, �������
                        if (string.IsNullOrWhiteSpace(nextLine) || !StartsWithIndent(nextLine))
                            break;

                        lines.Dequeue();
                    }
                    continue; // ��������� �� loop ��� ��� ������� ������
                }

                var matched = false;
                foreach (var ext in _extensions)
                {
                    if (ext.CanParse(line))
                    {
                        var blockElements = ext.ParseBlock(lines);
                        if (blockElements != null)
                        {
                            elements.AddRange(blockElements);
                            matched = true;
                            break;
                        }
                    }
                }

                if (!matched)
                {
                    // Default fallback: treat as paragraph
                    var paragraph = lines.Dequeue();
                    elements.Add(new MarkupElement { Tag = "p", Content = paragraph });
                }
            }

            return elements;
        }

        private bool StartsWithIndent(string line)
        {
            if (string.IsNullOrEmpty(line))
                return false;

            // ���������� true �� �� ����� char ����� ���� � tab
            return char.IsWhiteSpace(line[0]);
        }
    }
}
