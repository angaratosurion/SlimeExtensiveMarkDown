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
            // Αφαίρεσε YAML από το input
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

                // Αγνόησε ολόκληρο YAML block αν ξεκινάει από document_properties:
                if (line.StartsWith("document_properties:"))
                {
                    lines.Dequeue(); // Αφαίρεσε το document_properties:

                    // Αγνόησε όλες τις γραμμές με indent (κενά ή tab) ή μέχρι να τελειώσουν οι γραμμές
                    while (lines.Count > 0)
                    {
                        var nextLine = lines.Peek();

                        // Αν η γραμμή είναι κενή ή δεν έχει αρχικό indent, σταμάτα
                        if (string.IsNullOrWhiteSpace(nextLine) || !StartsWithIndent(nextLine))
                            break;

                        lines.Dequeue();
                    }
                    continue; // Επανέλαβε το loop για την επόμενη γραμμή
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

            // Επιστρέφει true αν το πρώτο char είναι κενό ή tab
            return char.IsWhiteSpace(line[0]);
        }
    }
}
