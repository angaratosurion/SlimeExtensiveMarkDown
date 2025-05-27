﻿using SlimeExtensiveMarkDown.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlimeExtensiveMarkDown.Core.Extensions
{
    public class LinkExtension : IBlockMarkupExtension
    {
        public bool CanParse(string line) => Regex.IsMatch(line, @"\[(.*?)\]\((.*?)\)");

        public MarkupElement? Parse(string line) => null;

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var line = lines.Dequeue();

            var textMatch = Regex.Match(line, @"\[(.*?)\]");
            var hrefMatch = Regex.Match(line, @"\((.*?)\)");
            var attrMatch = Regex.Match(line, @"\{(.*?)\}");

            var text = textMatch.Success ? textMatch.Groups[1].Value : "";
            var href = hrefMatch.Success ? hrefMatch.Groups[1].Value : "";

            var attributes = $"href=\"{href}\"";

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

            var html = $"<a {attributes}>{text}</a>";

            return new[] { new MarkupElement { Tag = "a", Content = html } };
        }
    }
}
