public class PostDTO
{
    public int? Id { get; set; }
    public string Username { get; set; }
    public string Text { get; set; }
    public string Timestamp { get; set; }

    //These two below fields may need to be extracted into separate extensions for specific types of DTO's
    public int? observationId { get; set; }
    public string? taxonId { get; set; }
}