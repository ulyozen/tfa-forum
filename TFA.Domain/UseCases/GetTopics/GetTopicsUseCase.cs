using FluentValidation;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Domain.UseCases.GetTopics;

internal partial class GetTopicsUseCase : IGetTopicsUseCase
{
    private readonly IValidator<GetTopicsQuery> _validator;
    private readonly IGetForumsStorage _getForumsStorage;
    private readonly IGetTopicsStorage _storage;
    
    public GetTopicsUseCase(IValidator<GetTopicsQuery> validator, IGetForumsStorage getForumsStorage, IGetTopicsStorage storage)
    {
        _validator = validator;
        _getForumsStorage = getForumsStorage;
        _storage = storage;
    }
    
    public async Task<(IEnumerable<Topic> resources, int totalCount)> Execute(GetTopicsQuery query, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);
        await _getForumsStorage.ThrowIfForumNotFound(query.ForumId, cancellationToken);
        
        return await _storage.GetTopics(query.ForumId, query.Skip, query.Take, cancellationToken);
    }
}