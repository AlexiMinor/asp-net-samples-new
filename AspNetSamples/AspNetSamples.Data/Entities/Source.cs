using AspNetSamples.Core;

namespace AspNetSamples.Data.Entities;

public class Source : IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Article> Articles { get; set; }
}