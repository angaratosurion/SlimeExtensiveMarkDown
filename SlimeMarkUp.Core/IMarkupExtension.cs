namespace SlimeExtensiveMarkDown.Core
{
    public interface IMarkupExtension
    {
        bool CanParse(string line);
        MarkupElement? Parse(string line);
    }
}
