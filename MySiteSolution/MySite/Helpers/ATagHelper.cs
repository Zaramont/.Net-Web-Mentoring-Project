using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyCatalogSite.Helpers
{
    [HtmlTargetElement(Attributes = "northwind-id")]
    public class ATagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.AllAttributes.TryGetAttribute("northwind-id", out TagHelperAttribute id);
            output.Attributes.RemoveAll("northwind-id");
            output.Attributes.Add("href", $"/images/{id.Value}");
        }
    }
}
