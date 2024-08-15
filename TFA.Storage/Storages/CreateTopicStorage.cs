using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Storage.Storages;

internal class CreateTopicStorage : ICreateTopicStorage
{
    private readonly IGuidFactory _guidFactory;
    private readonly IMomentProvider _momentProvider;
    private readonly ForumDbContext _forumDbContext;
    
    public CreateTopicStorage(IGuidFactory guidFactory, IMomentProvider momentProvider, ForumDbContext forumDbContext)
    {
        _guidFactory = guidFactory;
        _momentProvider = momentProvider;
        _forumDbContext = forumDbContext;
    }
    
    public Task<bool> ForumExist(Guid forumId, CancellationToken cancellationToken)
    {
        return _forumDbContext.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);
    }

    public async Task<Domain.Models.Topic> CreateTopic(Guid forumId, Guid userId, string title, CancellationToken cancellationToken)
    {
        var topicId = _guidFactory.Create();
        var topic = new Topic()
        {
            TopicId = topicId,
            ForumId = forumId,
            UserId = userId,
            Title = title,
            CreatedAt = _momentProvider.Now
        };

        await _forumDbContext.Topics.AddAsync(topic, cancellationToken);
        await _forumDbContext.SaveChangesAsync(cancellationToken);

        return await _forumDbContext.Topics
            .Where(t => t.TopicId == topicId)
            .Select(t => new Domain.Models.Topic
            {
                Id = t.TopicId,
                ForumId = t.ForumId,
                UserId = t.UserId,
                Title = t.Title,
                CreateAt = t.CreatedAt
            })
            .FirstAsync(cancellationToken);
    }
}