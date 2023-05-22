namespace AspNetSamples.WebAPI.Requests;

public class CreateOrUpdateArticleRequest
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string FullText { get; set; }
    public double Rate { get; set; }
    public string ArticleSourceUrl { get; set; }
    public int SourceId { get; set; }
}