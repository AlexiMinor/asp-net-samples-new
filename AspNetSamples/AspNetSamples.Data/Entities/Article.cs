namespace AspNetSamples.Data.Entities;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string FullText { get; set; }
    public double Rate { get; set;}

    public int SourceId { get; set; }
    public Source Source { get; set; }

}