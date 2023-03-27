namespace AspNetSamples.Core.DTOs
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullText { get; set; }
        public double Rate { get; set; }
        public int SourceId { get; set; }
        public string SourceName { get; set; }
    }

    public class SourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}