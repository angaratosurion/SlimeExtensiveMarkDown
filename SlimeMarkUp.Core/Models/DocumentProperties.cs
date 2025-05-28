namespace SlimeMarkUp.Core.Models
{
    public class DocumentProperties
    {
        public string? Filename { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public string? Keywords { get; set; }
        public string? Comments { get; set; }
        public string? Company { get; set; }
        public string? Category { get; set; }
        public string? Revision_Number { get; set; }
        public string? Language { get; set; }
        public List<string>? Contributors { get; set; }
        public string? Version_History { get; set; }
        public DateTime ? Published { get; set; }
    }
}
