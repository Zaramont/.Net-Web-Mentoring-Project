using System;
using System.Collections.Generic;
using System.IO;

namespace MyCatalogSite.Middleware
{
    public class FileCache
    {
        private readonly string pathToCacheDirectory;
        private readonly Dictionary<string, FileCacheEntry> _cache = new Dictionary<string, FileCacheEntry>();
        private const string fileExtension = ".bmp";
        private ushort maxFilesInCache;
        private TimeSpan expirationTime;

        public int CountOfFiles { get { return _cache.Count; } }

        public FileCache(string pathToCacheDirectory, ushort maxFilesInCache, TimeSpan expirationTime)
        {
            this.pathToCacheDirectory = pathToCacheDirectory;
            this.maxFilesInCache = maxFilesInCache;
            this.expirationTime = expirationTime;
            Directory.CreateDirectory(pathToCacheDirectory);
        }

        public bool TryGetFromCache(string id, out byte[] file)
        {
            file = null;
            string pathToFile = GetPathToFile(id);
            if (!string.IsNullOrEmpty(id) && _cache.ContainsKey(id) && File.Exists(pathToFile))
            {
                var entry = _cache[id];
                if (entry.ExpirationTime < DateTimeOffset.UtcNow)
                {
                    RemoveFromCache(id);
                    return false;
                }

                file = File.ReadAllBytes(pathToFile);
                return true;
            }

            return false;
        }

        public void SetToCache(string id, byte[] file)
        {
            var date = DateTimeOffset.UtcNow.Add(expirationTime);
            var newEntry = new FileCacheEntry { ExpirationTime = date };
            File.WriteAllBytes(GetPathToFile(id), file);
            _cache[id] = newEntry;
        }
        private void RemoveFromCache(string id)
        {
            File.Delete(GetPathToFile(id));
            _cache.Remove(id);
        }
        private string GetPathToFile(string id)
        {
            return Path.Combine(pathToCacheDirectory, id + fileExtension);
        }
    }

}