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
        private readonly object _cacheService;
        private readonly string _cacheFolder = Path.Combine(Environment.CurrentDirectory, "cache");

        public ImagesCacheMiddleware(RequestDelegate next)
        {
            this._next = next;
            Directory.CreateDirectory(_cacheFolder);
        }

        public async Task Invoke(HttpContext context)
        {
            string imageId = context.Request.RouteValues["id"].ToString();
            string pathToFile = Path.Combine(_cacheFolder, imageId + ".bmp");

            Stream originalBody = context.Response.Body;

            if (File.Exists(pathToFile))
            {
                var image = File.ReadAllBytes(pathToFile);

                using (var stream = new MemoryStream(image))
                {
                    await stream.CopyToAsync(originalBody);
                    context.Response.ContentType = "image/bmp";
                    context.Response.StatusCode = 200;
                    return;
                }
            }

            try
            {
                using (var swappedResponseStream = new MemoryStream())
                {
                    context.Response.Body = swappedResponseStream;

                    await _next(context);

                    if (context.Response.ContentType == "image/bmp")
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
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImagesCacheMiddleware>();
        }
    }

}