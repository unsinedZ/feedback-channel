using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackApi.Database
{
    public class ScriptCollector
    {
        public Task<IEnumerable<string>> ReadAllScriptsAsync(string catalogPath, string fileExtension = ".sql")
        {
            if (string.IsNullOrWhiteSpace(catalogPath))
                throw new ArgumentException("Catalog path is invalid.", nameof(catalogPath));

            if (string.IsNullOrWhiteSpace(fileExtension))
                throw new ArgumentException("File extension is invalid.", nameof(fileExtension));

            return Task.Run<IEnumerable<string>>(async () =>
            {
                if (!Directory.Exists(catalogPath))
                    throw new ArgumentException("Catalog does not exist.", nameof(catalogPath));

                var scriptPaths = Directory.EnumerateFiles(catalogPath, "*" + fileExtension.TrimEnd().TrimStart('*', ' '));
                return await Task.WhenAll(scriptPaths.Select(x => File.ReadAllTextAsync(x, Encoding.UTF8)));
            });
        }
    }
}