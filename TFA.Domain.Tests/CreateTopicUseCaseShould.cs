using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase _sut;
    
    private readonly Mock<ICreateTopicStorage> _storage = new();
    private readonly Mock<IIntentionManager> _intentionManager = new();
    
    private readonly ISetup<ICreateTopicStorage, Task<bool>> _forumExistSetup;
    private readonly ISetup<ICreateTopicStorage, Task<Models.Topic>> _createTopicSetup;
    private readonly ISetup<IIdentity, Guid> _getCurrentUser;
    private readonly ISetup<IIntentionManager, bool> _intentionAllowedSetup;
    
    public CreateTopicUseCaseShould()
    {
        _forumExistSetup = _storage.Setup(s => s.ForumExist(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _createTopicSetup = _storage.Setup(s => s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(i => i.Current).Returns(identity.Object);
        _getCurrentUser = identity.Setup(s => s.UserId);

        _intentionAllowedSetup = _intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntention>()));

        var validator = new Mock<IValidator<CreateTopicCommand>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        _sut = new CreateTopicUseCase(validator.Object, _intentionManager.Object, _storage.Object, identityProvider.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
    {
        var forumId = Guid.Parse("81E62D55-AA29-4107-B93F-599341D3AB56");
        
        _intentionAllowedSetup.Returns(false);

        await _sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Whatever"), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
        
        _intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
    }
    
    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        var forumId = Guid.Parse("1C145780-E8E3-42E9-8145-417483A993F9");
        _intentionAllowedSetup.Returns(true);
        _forumExistSetup.ReturnsAsync(false);

        await _sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Some title"), CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
        
        _storage.Verify(s => s.ForumExist(forumId, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExist()
    {
        var forumId = Guid.Parse("12410FF1-C185-4494-A903-E33214A47C80");
        var userId = Guid.Parse("8B33DDCE-E7CF-478C-99F2-2045E1C070B3");
        
        _intentionAllowedSetup.Returns(true);
        _forumExistSetup.ReturnsAsync(true);
        _getCurrentUser.Returns(userId);
        
        var expected = new Models.Topic();
        _createTopicSetup.ReturnsAsync(expected); 
        
        var actual = await _sut.Execute(new CreateTopicCommand(forumId, "Hello world"), CancellationToken.None);

        actual.Should().Be(expected);
        
        _storage.Verify(s => s.CreateTopic(forumId, userId, "Hello world", It.IsAny<CancellationToken>()), Times.Once);
    }
}