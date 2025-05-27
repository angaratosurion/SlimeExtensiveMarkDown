using System;
using System.IO;
using System.Collections.Generic;
using SlimeMarkUp.Core;
using SlimeMarkUp.Core.Extensions;

namespace SlimeMarkUp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new MarkupParser(new List<IBlockMarkupExtension>
            {
                new HeaderExtension(),
                new ImageExtension(),
                new TableExtension(),
                new ListExtension(),
                new CodeBlockExtension(),
                new BlockquoteExtension(),
                new InlineStyleExtension(),
                new LinkExtension()
            });

            string input = File.ReadAllText("input.txt");
            var elements = parser.Parse(input);

            var renderer = new HtmlRenderer();
            string html = renderer.Render(elements);

            File.WriteAllText("output.html", html);

            Console.WriteLine("HTML export complete. Check output.html");
        }
    }
}
