﻿namespace AspNetSamples.Mvc.Models;

public class ArticlesWithPaginationModel
{
    public List<ArticlePreviewModel> ArticlePreviews { get; set; }
    public PageInfo PageInfo { get; set; }
}