﻿using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetSamples.Mvc.Helpers.TagHelpers;

public class PaginationTagHelper : TagHelper
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    public PageInfo PageInfo { get; set; }
    public string PageAction { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }


    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
        var result = new TagBuilder("div");

        result.AddCssClass("btn-group");

        for (int i = 1; i <= PageInfo.TotalPages; i++)
        {
            var tag = new TagBuilder("a");
            var anchorInnerHtml = i.ToString();
            tag.AddCssClass("btn btn-outline-primary");
            if (ViewContext.HttpContext.Request.Query.ContainsKey("page") && int.TryParse(
                    ViewContext.HttpContext.Request.Query["page"],
                    out var actualPage))
            {
                if (i == actualPage)
                {
                    tag.AddCssClass("active");
                }
            }
            else
            {
                if (i == 1)
                {
                    tag.AddCssClass("active");
                }
            }
            tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
            tag.InnerHtml.Append(anchorInnerHtml);
            result.InnerHtml.AppendHtml(tag);
        }

        output.TagName = "div";

        output.Content.AppendHtml(result.InnerHtml);
    }
}