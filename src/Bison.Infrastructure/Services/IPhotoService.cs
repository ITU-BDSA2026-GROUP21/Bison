public interface IPhotoService
{
    public Task<string> UploadPhotoAsync(Stream photo); // Uploads the given photo to the file system and returns the generated filename. Should probably throw a specific exception on a failure
    public void DeletePhoto(string fileName); // Deletes the photo with the given fileName from the file system.
}