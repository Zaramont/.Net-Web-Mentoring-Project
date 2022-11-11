namespace MyCatalogSite.Models
{
    public class BreadCrumbPart
    {
        public string Text { get; set; }
        public string RelativePath  { get; set; }
        public bool IsLastPart { get; set; }
    }
}
