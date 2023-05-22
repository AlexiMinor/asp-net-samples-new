namespace AspNetSamples.WebAPI.Responses;

public class ArticleResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string FullText { get; set; }
    public double Rate { get; set; }
    public string ArticleSourceUrl { get; set; }
    public int SourceId { get; set; }
    public string SourceName { get; set; }
}