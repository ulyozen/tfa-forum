using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.UseCases.CreateForum;
using TFA.Storage.Storages;

namespace TFA.Storage;

internal class CreateForumStorage : ICreateForumStorage
{
    private readonly IMemoryCache _memoryCache;
    private readonly IGuidFactory _guidFactory;
    private readonly ForumDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateForumStorage(
        IMemoryCache memoryCache,
        IGuidFactory guidFactory,
        ForumDbContext dbContext,
        IMapper mapper)
    {
        _memoryCache = memoryCache;
        _guidFactory = guidFactory;
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<Domain.Models.Forum> Create(string title, CancellationToken cancellationToken)
    {
        var forumId = _guidFactory.Create();
        var forum = new Forum()
        {
            ForumId = forumId,
            Title = title
        };

        await _dbContext.Forums.AddAsync(forum, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _memoryCache.Remove(nameof(GetForumsStorage.GetForums));

        return await _dbContext.Forums
            .Where(f => f.ForumId == forumId)
            .ProjectTo<Domain.Models.Forum>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}