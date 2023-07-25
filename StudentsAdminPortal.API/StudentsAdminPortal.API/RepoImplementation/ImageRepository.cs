using StudentsAdminPortal.API.Repositories;

namespace StudentsAdminPortal.API.RepoImplementation
{
    public class ImageRepository : IImageRepository
    {
        public async Task<string> UploadImage(IFormFile file, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    @"Resources\Images", fileName);
            using Stream fileStream = new FileStream(
                filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return getRelativeFilePath(fileName);
        }
        private string getRelativeFilePath(string fileName)
        {
            return Path.Combine(@"Resources\Images",fileName);
        }
    }
}
