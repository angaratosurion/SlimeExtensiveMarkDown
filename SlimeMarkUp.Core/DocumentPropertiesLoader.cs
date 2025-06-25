using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using SlimeMarkUp.Core.Models;

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
        public static string? CommentProperties(string input, DocumentProperties prop)
        {
            string ap = null;

            //DocumentProperties
            if ( prop !=null)
            {
                var serializer=  new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
                
            }


            return ap;


            

        }
    }
}
