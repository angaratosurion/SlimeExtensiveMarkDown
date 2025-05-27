using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions
{
    public class InlineStyleExtension : IBlockMarkupExtension
    {
        public bool CanParse(string line) =>
            line.Contains("**") || line.Contains("*");

        public MarkupElement? Parse(string line)
        {
            var content = ApplyInlineStyles(line);
            return new MarkupElement
            {
                Tag = "p",
                Content = content
            };
        }

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0)
                return null;

            var line = lines.Dequeue();
            if (!CanParse(line))
                return null;

            var content = ApplyInlineStyles(line);
            return new[]
            {
                new MarkupElement
                {
                    Tag = "p",
                    Content = content
                }
            };
        }

        private string ApplyInlineStyles(string input)
        {
            var result = input;
            result = Regex.Replace(result, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
            result = Regex.Replace(result, @"\*(.+?)\*", "<em>$1</em>");
            return result;
        }
    }
}
