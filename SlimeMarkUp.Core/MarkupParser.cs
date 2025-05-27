using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeMarkUp.Core
{
    public class MarkupParser
    {
        private readonly List<IBlockMarkupExtension> _extensions;

        public MarkupParser(IEnumerable<IBlockMarkupExtension> extensions)
        {
            _extensions = extensions.ToList();
        }

        public List<MarkupElement> Parse(string text)
        {
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
    }
}
