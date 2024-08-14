using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

public class GetForumUseCase : IGetForumUseCase
{
    private readonly IGetForumStorage _storage;

    public GetForumUseCase(IGetForumStorage storage)
    {
        _storage = storage;
    }

    public Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken) => 
        _storage.GetForums(cancellationToken);
}