namespace TFA.API.Models;

public class Topic
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}