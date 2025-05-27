using SlimeExtensiveMarkDown.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeExtensiveMarkDown.Core.Extensions
{
    public class ImageExtension : IBlockMarkupExtension
    {
        public bool CanParse(string line) => line.StartsWith("![");

        public MarkupElement? Parse(string line) => null; // Δεν χρησιμοποιείται

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var line = lines.Dequeue();

            var altMatch = Regex.Match(line, @"!\[(.*?)\]");
            var srcMatch = Regex.Match(line, @"\((.*?)\)");
            var attrMatch = Regex.Match(line, @"\{(.*?)\}");

            var alt = altMatch.Success ? altMatch.Groups[1].Value : "";
            var src = srcMatch.Success ? srcMatch.Groups[1].Value : "";

            var attributes = $"src=\"{src}\" alt=\"{alt}\"";

            if (attrMatch.Success)
            {
                var attrParts = attrMatch.Groups[1].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in attrParts)
                {
                    var kv = part.Split('=');
                    if (kv.Length == 2)
                    {
                        var key = kv[0];
                        var value = kv[1];
                        attributes += $" {key}=\"{value}\"";
                    }
                }
            }

            var html = $"<img {attributes} />";

            return new[] { new MarkupElement { Tag = "img", Content = html } };
        }
    }
}
