namespace SlimeMarkUp.Core
{
    public interface IMarkupExtension
    {
        bool CanParse(string line);
        MarkupElement? Parse(string line); // για inline
    }

    public interface IBlockMarkupExtension : IMarkupExtension
    {
        IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines);
    }

}
