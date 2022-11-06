using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyCatalogSite.Middleware
{
    public class ImagesCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ImagesCacheOptions _options;
        private readonly FileCache _fileCache;

        public ImagesCacheMiddleware(RequestDelegate next, ImagesCacheOptions options)
        {
            this._next = next;
            this._options = options;
            this._fileCache = new FileCache(options.CacheFolder, options.MaxImagesCount, TimeSpan.FromSeconds(options.ExpirationTimeInSeconds));
        }

        public async Task Invoke(HttpContext context)
        {
            string imageId = context.Request.RouteValues["id"].ToString();

            if (_fileCache.TryGetFromCache(imageId, out byte[] image))
            {
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
                        context.Response.ContentType == "image/bmp" &&
                        _fileCache.CountOfFiles < _options.MaxImagesCount)
                    {
                        swappedResponseStream.Position = 0;
                        var responseBody = swappedResponseStream.ToArray();
                        _fileCache.SetToCache(imageId, responseBody);

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