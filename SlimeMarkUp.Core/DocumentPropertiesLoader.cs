using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using SlimeMarkUp.Core.Models;
using System.Net.WebSockets;

namespace SlimeMarkUp.Core
{
    public static class DocumentPropertiesLoader
    {
        public static DocumentProperties? Load(string input)
        {
            // Εντοπίζει το YAML front matter: ξεκινά και τελειώνει με ---
            var match = Regex.Match(input, @"^\s*---\s*\r?\n(.*?)\r?\n\s*---", 
                RegexOptions.Singleline);

            if (!match.Success)
                return null;

            var yaml = match.Groups[1].Value;

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance) // filename → FileName
                .IgnoreUnmatchedProperties()
                .Build();

            try
            {
                return deserializer.Deserialize<DocumentProperties>(yaml);
            }
            catch
            {
                return null;
            }
        }
        public static string ? SerializeProperties(DocumentProperties prop)
        {
            string ap = null;
            if (prop != null)
            {
                var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
                    
                .Build();
                var tmp = serializer.Serialize(prop);
                ap = tmp;

            }
            return ap;
        }
        public static string? CommentProperties(DocumentProperties prop)
        {
            string ap = null;

            //DocumentProperties
            if ( prop !=null)
            {
                 
                var tmp= SerializeProperties(prop);
                ap = "<!-- \n"+tmp + "\n-->";

            }


            return ap;


            

        }
        public static void SaveToFile(string filename , DocumentProperties prop)
        {
            if (prop != null && !string.IsNullOrEmpty(filename))
            {
                var cont = SerializeProperties(prop);
                if ( cont != null )
                {
                    File.WriteAllText(filename, cont);  

                }
            }
        }
    }
}
