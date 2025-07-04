using System.Collections.Generic;

namespace SlimeMarkUp.Core.Extensions.SlimeMarkup
{
    public class HeaderExtension : IBlockMarkupExtension
    {
        public int Count { get; }
        public bool CanParse(string line) => line.TrimStart().StartsWith("#");
        public bool IsToBeProccessed
        { get { return false; } }
        public MarkupElement? Parse(string line)
        {
            int level = 0;
            while (level < line.Length && line[level] == '#') level++;

            var content = line.Substring(level).Trim();

            return new MarkupElement
            {
                Tag = $"h{level}",
                Content = content
            };
        }

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            if (lines.Count == 0) return null;
            var line = lines.Dequeue();
            return new[] { Parse(line) };
        }
    
    public bool Priority()
        {
            return false;
        }
        public int Order
        {
            get
            {
                return 2;
            }
        }
    } }
