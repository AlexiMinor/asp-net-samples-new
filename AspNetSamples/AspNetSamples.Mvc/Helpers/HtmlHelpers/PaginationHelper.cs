using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetSamples.Mvc.Helpers.HtmlHelpers;

public static class PaginationHelper
{
    public static HtmlString Pagination(this IHtmlHelper htmlHelper,
        int currentPage,
        int pagesCount)
    {
        var resultString = new StringBuilder();

        resultString.Append("<div>");
        resultString.Append("<p>1,2,3</p>");
        resultString.Append("</div>");

        return new HtmlString(resultString.ToString());
    }
}