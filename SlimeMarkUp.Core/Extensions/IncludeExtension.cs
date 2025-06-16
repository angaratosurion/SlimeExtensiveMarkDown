using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SlimeMarkUp.Core.Extensions
{
    public class IncludeExtension : IBlockMarkupExtension
    {
        public bool Priority()
        {
            return true;
        }
        private static readonly Regex IncludeRegex = new(@"<!--\s*include:\s*(.+?)\s*-->", RegexOptions.Compiled);

        public bool CanParse(string line)
        {
            return IncludeRegex.IsMatch(line);
        }

        public MarkupElement? Parse(string line)
        {
            var match = IncludeRegex.Match(line);
            if (!match.Success)
                return null;

            var inputPath = match.Groups[1].Value.Trim();
            string fullPath = Path.GetFullPath(inputPath);

            if (!File.Exists(fullPath))
            {
                return new MarkupElement
                {
                    Tag = "p",
                    Content = $"<!-- ERROR: File '{inputPath}' not found -->"
                };
            }

            try
            {
                var content = File.ReadAllText(fullPath);
                return new MarkupElement
                {
                    Tag = "raw",
                    Content = content
                };
            }
            catch (Exception ex)
            {
                return new MarkupElement
                {
                    Tag = "p",
                    Content = $"<!-- ERROR: Could not read '{inputPath}': {ex.Message} -->"
                };
            }
        }

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var line = lines.Dequeue();
            if (!CanParse(line))
                return null;

            var element = Parse(line);
            return element != null ? new[] { element } : null;
        }
    }
}
