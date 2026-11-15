public class PhotoService : IPhotoService
{
    private readonly string _imageDirectory;
    public PhotoService(string imageDirectory)
    {
        _imageDirectory = imageDirectory;
    }
    public async Task<string> UploadPhotoAsync(Stream photo)
    {
        throw new NotImplementedException();
    }
    public async Task DeletePhotoAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}