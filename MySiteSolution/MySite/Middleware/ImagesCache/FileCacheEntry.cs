using System;

namespace MyCatalogSite.Middleware
{
    public class FileCacheEntry
    {
        public DateTimeOffset ExpirationTime { get; set; }
    }

}