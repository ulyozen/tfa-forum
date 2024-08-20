using FluentValidation;
using TFA.Domain.Authorization;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumUseCase : ICreateForumUseCase
{
    private readonly IValidator<CreateForumCommand> _validator;
    private readonly IIntentionManager _intentionManager;
    private readonly ICreateForumStorage _storage;

    public CreateForumUseCase(IValidator<CreateForumCommand> validator, IIntentionManager intentionManager, ICreateForumStorage storage)
    {
        _validator = validator;
        _intentionManager = intentionManager;
        _storage = storage;
    }

    public async Task<Forum> Execute(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        _intentionManager.ThrowIfForbidden(ForumIntention.Create);

        return await _storage.Create(command.Title, cancellationToken);
    }
}