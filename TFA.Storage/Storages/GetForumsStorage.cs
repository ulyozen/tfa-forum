using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage.Storages;

internal class GetForumsStorage : IGetForumsStorage
{
    private readonly IMemoryCache _memoryCache;
    private readonly ForumDbContext _forumDbContext;
    private readonly IMapper _mapper;

    public GetForumsStorage(IMemoryCache memoryCache, ForumDbContext forumDbContext, IMapper mapper)
    {
        _memoryCache = memoryCache;
        _forumDbContext = forumDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken) =>
        await _memoryCache.GetOrCreateAsync<Domain.Models.Forum[]>(
            nameof(GetForums),
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return _forumDbContext.Forums
                    .ProjectTo<Domain.Models.Forum>(_mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);
            }) ?? Array.Empty<Domain.Models.Forum>();

}