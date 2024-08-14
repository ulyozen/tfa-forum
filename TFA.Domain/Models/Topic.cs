namespace TFA.Domain.Models;

public class Topic
{
    public Guid Id { get; set; }
    
    public Guid ForumId { get; set; }
    
    public Guid UserId { get; set; }

    public string Title { get; set; } = string.Empty;
    
    public DateTimeOffset CreateAt { get; set; }
}