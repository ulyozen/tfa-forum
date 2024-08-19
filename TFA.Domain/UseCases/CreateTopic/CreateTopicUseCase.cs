using FluentValidation;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.GetForums;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> _validator;
    private readonly IIntentionManager _intentionManager;
    private readonly ICreateTopicStorage _storage;
    private readonly IGetForumsStorage _getForumsStorage;
    private readonly IIdentityProvider _identityProvider;

    public CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        ICreateTopicStorage storage,
        IGetForumsStorage getForumsStorage,
        IIdentityProvider identityProvider)
    {
        _validator = validator;
        _intentionManager = intentionManager;
        _storage = storage;
        _getForumsStorage = getForumsStorage;
        _identityProvider = identityProvider;
    }
    
    public async Task<Topic> Execute(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var (forumId, title) = command;
        _intentionManager.ThrowIfForbidden(TopicIntention.Create);

        await _getForumsStorage.ThrowIfForumNotFound(forumId, cancellationToken);

        return await _storage.CreateTopic(forumId, _identityProvider.Current.UserId, title, cancellationToken);
    }
}