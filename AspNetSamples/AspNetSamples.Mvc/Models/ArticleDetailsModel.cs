using AspNetSamples.Core.DTOs;

namespace AspNetSamples.Mvc.Models;

public class ArticleDetailsModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FullText { get; set; }
    public double Rate { get; set; }
    public string SourceName { get; set; }
}