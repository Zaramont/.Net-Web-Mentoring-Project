using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace MyCatalogSite.Helpers
{
    public static class CustomHTMLHelpers
    {
        public static IHtmlContent NorthwindImageLink(this IHtmlHelper htmlHelper, int id, string linkName)
        {
            return new HtmlString($"<a href=\"/images/{id}\">{linkName}</a>");
        }

        /*public static String HelloWorldString(this IHtmlHelper htmlHelper)
            => "<strong>Hello World</strong>";*/

    }
}
