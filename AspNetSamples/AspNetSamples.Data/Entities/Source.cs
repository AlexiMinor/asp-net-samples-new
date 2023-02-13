namespace AspNetSamples.Data.Entities;

public class Source
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Article> Articles { get; set; }
}