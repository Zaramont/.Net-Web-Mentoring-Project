using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyCatalogSite.Middleware
{
    public class ImagesCacheOptions
    {
        private const string DefaultCacheFolder = "cache";
        private const ushort DefaultImagesCount = 25;
        private const ushort DefaultExpirationTime = 60;
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

    public class ImagesCacheMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ImagesCacheOptions _options;

        public ImagesCacheMiddleware(RequestDelegate next, ImagesCacheOptions options)
        {
            this._next = next;
            this._options = options;
        }

        public async Task Invoke(HttpContext context)
        {
        //TODO add support for maxImages and expirationTime
        //refactor the method
            Directory.CreateDirectory(_options.CacheFolder);
            string imageId = context.Request.RouteValues["id"].ToString();
            string pathToFile = Path.Combine(_options.CacheFolder, imageId + ".bmp");


            if (File.Exists(pathToFile))
            {
                var image = File.ReadAllBytes(pathToFile);

                using (var stream = new MemoryStream(image))
                {
                    context.Response.ContentType = "image/bmp";
                    await stream.CopyToAsync(context.Response.Body);
                    return;
                }
            }

            Stream originalBody = context.Response.Body;
            try
            {
                using (var swappedResponseStream = new MemoryStream())
                {
                    context.Response.Body = swappedResponseStream;

                    await _next(context);

                    if (context.Request.Method == HttpMethods.Get &&
                        context.Response.StatusCode == 200 &&
                        context.Response.ContentType == "image/bmp")
                    {
                        swappedResponseStream.Position = 0;
                        var responseBody = swappedResponseStream.ToArray();
                        File.WriteAllBytes(pathToFile, responseBody);

                        swappedResponseStream.Position = 0;
                        await swappedResponseStream.CopyToAsync(originalBody);
                    }
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }

    public static class ImagesCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseImagesCache(
            this IApplicationBuilder builder, Action<ImagesCacheOptions> configureOptions)
        {
            var options = new ImagesCacheOptions();
            configureOptions(options);

            return builder.UseMiddleware<ImagesCacheMiddleware>(options);
        }
    }

}