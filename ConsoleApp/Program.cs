using SlimeMarkUp.Core;
using SlimeMarkUp.Core.Extensions;
using SlimeMarkUp.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

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
                new LinkExtension(),
                 new IncludeExtension()

            });

            string input = File.ReadAllText("input.txt");
            
            var props =  DocumentPropertiesLoader.Load(input);

             
            DocumentProperties? docProps = props;
            var elements = parser.Parse(input);

            var renderer = new HtmlRenderer();
            string html = renderer.Render(elements);
            elements= parser.Parse(html);
            html = renderer.Render(elements);


            File.WriteAllText("output.html", html);
            if (docProps != null)
            {
                Console.WriteLine($"File Name: {docProps.Filename}");
                Console.WriteLine($"Author: {docProps.Author}");
                // κλπ...
            }
            else
            {
                Console.WriteLine("Δεν βρέθηκαν ιδιότητες εγγράφου.");
            }
            Console.WriteLine("HTML export complete. Check output.html");
        }
    }
}
