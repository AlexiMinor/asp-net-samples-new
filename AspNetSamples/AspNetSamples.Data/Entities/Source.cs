﻿using AspNetSamples.Core;

namespace AspNetSamples.Data.Entities;

public class Source : IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string RssFeedUrl { get; set; }
    public string OriginUrl { get; set; }

    public List<Article> Articles { get; set; }
}