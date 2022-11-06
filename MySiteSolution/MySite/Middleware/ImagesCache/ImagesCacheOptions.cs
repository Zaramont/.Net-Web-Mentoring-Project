using System;
using System.IO;

namespace MyCatalogSite.Middleware
{
    public class ImagesCacheOptions
    {
        private const string DefaultCacheFolder = "cache";
        private const ushort DefaultImagesCount = 25;
        private const ushort DefaultExpirationTime = 30;
        public ImagesCacheOptions()
        {
            CacheFolder = Path.Combine(Environment.CurrentDirectory, DefaultCacheFolder);
            MaxImagesCount = DefaultImagesCount;
            ExpirationTimeInSeconds = DefaultExpirationTime;
        }
        public string CacheFolder { get; set; }
        public ushort MaxImagesCount { get; set; }
        public ushort ExpirationTimeInSeconds { get; set; }
    }

}