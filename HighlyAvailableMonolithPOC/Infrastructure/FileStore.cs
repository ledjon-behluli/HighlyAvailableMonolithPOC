using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HighlyAvailableMonolithPOC.Infrastructure
{
    public class FileStore
    {
        public async Task Add(Stream content, string destinationPath, string destinationFileName)
        {
            await Task.Delay(3);  // Simulate some long running task.

            Directory.CreateDirectory(destinationPath);
            using (var stream = File.Create(destinationFileName))
            {
                await content.CopyToAsync(stream);
            }
        }

        public async Task Remove(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                await Task.Delay(3);        // Simulate some long running task.

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
