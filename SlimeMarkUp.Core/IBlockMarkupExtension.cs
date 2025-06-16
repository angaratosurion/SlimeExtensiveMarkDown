namespace SlimeMarkUp.Core
{
    public interface IMarkupExtension
    {
        bool CanParse(string line);
        MarkupElement? Parse(string line); // ��� inline
        bool Priority();
    }

    public interface IBlockMarkupExtension : IMarkupExtension
    {
        IEnumerable<MarkupElement>? ParseBlock(Queue<string> lines);
    }

}
