namespace SlimeExtensiveMarkDown.Core
{
    public class MarkupElement
    {
        public string Tag { get; set; } = "p";
        public string Content { get; set; } = "";
        public Dictionary<string, string>? Attributes { get; set; }
        public List<MarkupElement>? Children { get; set; }
    }
}
