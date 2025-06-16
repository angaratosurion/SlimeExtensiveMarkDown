using System.Collections.Generic;
using SlimeMarkUp.Core;
using System.Linq;

namespace SlimeMarkUp.Core.Extensions
{
    public class TableExtension : IBlockMarkupExtension
    {
        public bool Priority()
        {
            return false;
        }
        public bool CanParse(string line) => line.TrimStart().StartsWith("|");

        public MarkupElement? Parse(string line) => null; // Δεν χρησιμοποιείται

        public IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines)
        {
            var rows = new List<List<string>>();

            while (lines.Count > 0 && lines.Peek().TrimStart().StartsWith("|"))
            {
                var line = lines.Dequeue().Trim();
                // Αγνόησε γραμμές separator (μόνο - ή =)
                var cells = line.Trim('|').Split('|').Select(c => c.Trim()).ToList();
                if (cells.All(c => c.All(ch => ch == '-' || ch == '=')))
                    continue;
                rows.Add(cells);
            }

            var htmlRows = rows.Select(row =>
                "<tr>" + string.Join("", row.Select(cell => $"<td>{cell}</td>")) + "</tr>"
            );

            var html = "<table>" + string.Join("", htmlRows) + "</table>";

            return new[] { new MarkupElement { Tag = "table", Content = html } };
        }
    }
}
