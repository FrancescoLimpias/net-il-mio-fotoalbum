using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Cryptography;

namespace il_mio_fotoalbum.Utilities
{
    public static class UploadsManager
    {

        public static readonly string UploadsPath = "wwwroot/uploads/";
        private static readonly PhotoAlbumContext context = new();

        public static string HashBytes(byte[] bytes)
        {
            using var sha256 = SHA256.Create();

            // calculate the hash of the file content
            byte[] hashBytes = sha256.ComputeHash(bytes);
            string fileHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return fileHash;
        }

        public static long FileReferencesCount(string fileName)
        {
            return context.Photos.Where(photo => photo.Location == fileName).Count();
        }

        private static void Write(string fileName, Action<string> writeAction)
        {
            if (FileReferencesCount(fileName) == 0)
            {
                var filePath = Path.Combine(UploadsPath, fileName);
                writeAction(filePath);
            }

        }

        public static string Write(IFormFile formFile)
        {
            string fileName;
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                fileName = HashBytes(memoryStream.ToArray()) + Path.GetExtension(formFile.FileName);
            }

            Write(fileName, (filePath) =>
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                formFile.CopyTo(stream);
            });

            return fileName;
        }

        public static async Task<string> Download(string url)
        {
            using HttpClient client = new HttpClient();

            // Download the image from the specified URL
            HttpResponseMessage response = await client.GetAsync(url);
            Uri finalUrl = response.Headers.Location;
            byte[] imageData = await client.GetByteArrayAsync(finalUrl);

            string imageDataHash = HashBytes(imageData);

            // Save the image to the file system
            string fileName = imageDataHash + ".jpg"; // Use the hash as the filename

            Write(fileName, (filePath) =>
            {
                File.WriteAllBytes(filePath, imageData);
            });

            return fileName;
        }

        public static void Delete(string fileName)
        {
            try
            {
                File.Delete(Path.Combine(UploadsPath, fileName));
            }
            catch { }
        }

        public static bool AttemptDelete(string fileName)
        {
            if (FileReferencesCount(fileName) == 1)
            {
                Delete(fileName);
                return true;
            }

            return false;
        }

    }
}
