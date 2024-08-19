using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

internal class GetForumUseCase : IGetForumUseCase
{
    private readonly IGetForumsStorage _storage;

    public GetForumUseCase(IGetForumsStorage storage)
    {
        _storage = storage;
    }

    public Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken) => 
        _storage.GetForums(cancellationToken);
}