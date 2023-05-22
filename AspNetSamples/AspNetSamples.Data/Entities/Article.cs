using AspNetSamples.Core;

namespace AspNetSamples.Data.Entities;

public class Article : IBaseEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string FullText { get; set; }
    public double? Rate { get; set;}

    public string ArticleSourceUrl { get; set; }

    public int SourceId { get; set; }
    public Source Source { get; set; }

    public virtual List<Comment> Comments { get; set; }

}