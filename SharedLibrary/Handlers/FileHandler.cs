using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public static class FileHandler
    {
        public async static Task<MemoryStream> GetStreamAsync(this IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return stream;
        }


        public async static Task<byte[]> GetBytesAsync(IFormFile file)
        {
            return (await file.GetStreamAsync()).ToArray();
        }


        public static MemoryStream GetStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }


        public async static Task<MemoryStream> GetStreamAsync(string filePath)
        {
            using var stream = new MemoryStream();
            using FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            await file.CopyToAsync(stream);
            return stream;
        }


        public async static Task<byte[]> GetBytesAsync(string filePath)
        {
            return (await GetStreamAsync(filePath)).ToArray();
        }
    }
}
