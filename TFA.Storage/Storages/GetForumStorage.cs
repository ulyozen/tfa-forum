using Microsoft.EntityFrameworkCore;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Storage.Storages;

public class GetForumStorage : IGetForumStorage
{
    private readonly ForumDbContext _forumDbContext;

    public GetForumStorage(ForumDbContext forumDbContext)
    {
        _forumDbContext = forumDbContext;
    }

    public async Task<IEnumerable<Domain.Models.Forum>> GetForums(CancellationToken cancellationToken)
    {
        return await _forumDbContext.Forums
            .Select(f => new Domain.Models.Forum
            {
                Id = f.ForumId,
                Title = f.Title
            })
            .ToArrayAsync(cancellationToken);
    }
}