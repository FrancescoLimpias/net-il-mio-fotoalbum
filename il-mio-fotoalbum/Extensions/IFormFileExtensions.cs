using System.Security.Cryptography;

namespace ExtensionMethods
{
    public static class IFormFileExtensions
    {

        public static string GetContentHash(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            using (var sha256 = SHA256.Create())
            {
                // get file content into memory
                formFile.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                // calculate the hash of the file content
                byte[] hashBytes = sha256.ComputeHash(fileBytes);
                string fileHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return fileHash;
            }
        }

        public static void WriteToUploads(this IFormFile formFile, string fileName)
        {
            //Save the image file (with updated name)
            var filePath = Path.Combine("wwwroot/uploads/", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
        }
    }
}
