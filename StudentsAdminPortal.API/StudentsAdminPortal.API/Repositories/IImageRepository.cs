namespace StudentsAdminPortal.API.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadImage(IFormFile file, string fileName);
    }
}
