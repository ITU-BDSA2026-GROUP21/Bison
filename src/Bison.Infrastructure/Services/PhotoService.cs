// This class doesn't do a complete image validation, just a cursory check to see if it matches the file format. 
// Maybe we should consider importing a proper image checking library.

public class PhotoService : IPhotoService
{
    private readonly string _imageDirectory;
    private const long MaxPhotoSize = 10 * 1024 * 1024;
    private const long MinPhotoSize = 12;
    public PhotoService(string imageDirectory)
    {
        _imageDirectory = imageDirectory;
    }
    public async Task<string> UploadPhotoAsync(Stream photo)
    {
        // Check if photo has a valid size
        if (photo.Length > MaxPhotoSize)
        {
            throw new ArgumentException("Photo exceeds the maximum size of allowed uploads!");
        }
        else if (photo.Length < MinPhotoSize)
        {
            throw new ArgumentException("Photo is too small and is invalid!");
        }

        // Set Stream position to start, read the header of the photo to check file signature, and reset the Stream's position back to start.
        photo.Position = 0;
        byte[] header = new byte[12];
        await photo.ReadExactlyAsync(header);
        photo.Position = 0;

        // Define signatures of valid photo formats, in pairs of hexadecimals.
        byte[] jpegSignature = [0xFF, 0xD8, 0xFF];
        byte[] pngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
        byte[] webpRiffSignature = [0x52, 0x49, 0x46, 0x46];
        byte[] webpFormatSignature = [0x57, 0x45, 0x42, 0x50];

        // Check if the uploaded file matches one of the approved image formats.
        bool matchesJpeg = header.AsSpan(0, 3).SequenceEqual(jpegSignature);
        bool matchesPng = header.AsSpan(0, 8).SequenceEqual(pngSignature);
        bool matchesWebpRiff = header.AsSpan(0, 4).SequenceEqual(webpRiffSignature);
        bool matchesWebpFormat = header.AsSpan(8, 4).SequenceEqual(webpFormatSignature);

        // Create file extension string based on matching image formats. If none match, throw an ArgumentException.
        string fileExtension;
        if (matchesJpeg)
        {
            fileExtension = ".jpeg";
        }
        else if (matchesPng)
        {
            fileExtension = ".png";
        }
        else if (matchesWebpRiff && matchesWebpFormat)
        {
            fileExtension = ".webp";
        }
        else
        {
            throw new ArgumentException("The uploaded file is not one of the approved image formats!");
        }

        // Generate file name with directory and extension
        string fileName = Guid.NewGuid().ToString();
        string fileNameWithExtension = fileName + fileExtension;
        string fullPath = Path.Combine(_imageDirectory, fileNameWithExtension);

        // Create file with full file path, including extension, then copy to new file. If for some reason it throws, it deletes the possibly partially written file.
        // Then throws the exception on to the caller.
        try
        {

            await using FileStream actualFile = File.Create(fullPath);
            await photo.CopyToAsync(actualFile);

        }
        catch (Exception)
        {
            File.Delete(fullPath);
            throw;
        }

        // Return the generated filename to caller. Only the actual file name is returned, not the full path, since we only store the file name in the DB.
        return fileNameWithExtension;
    }
    public void DeletePhoto(string fileName)
    {
        // Check if the filename contains any slashes that could make the full path point somewhere incorrectly. In case it does, throw an ArgumentException.
        if (fileName.Contains('/') || fileName.Contains('\\'))
        {
            throw new ArgumentException("The given fileName could potentially point outside the uploads directory!");
        }

        string fullPath = Path.Combine(_imageDirectory, fileName);

        File.Delete(fullPath);
    }
}