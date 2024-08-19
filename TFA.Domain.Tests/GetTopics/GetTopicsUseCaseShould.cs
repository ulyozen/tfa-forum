using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics;

public class GetTopicsUseCaseShould
{
    private readonly GetTopicsUseCase _sut;
    private readonly Mock<IGetTopicsStorage> _storage;
    private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> _getTopicSetup;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> _getForumsSetup;

    public GetTopicsUseCaseShould()
    {
        var validator = new Mock<IValidator<GetTopicsQuery>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var getForumsStorage = new Mock<IGetForumsStorage>();
        _getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

        _storage = new Mock<IGetTopicsStorage>();
        _getTopicSetup = _storage.Setup(s => s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));
        
        _sut = new GetTopicsUseCase(validator.Object, getForumsStorage.Object, _storage.Object);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenForumNotFound()
    {
        var forumId = Guid.Parse("C17497BC-6467-415A-BF7C-B17C39E97BB0");

        _getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = Guid.Parse("2902D0CB-2C03-4F9C-A96F-8C07106B33A1") } });

        var query = new GetTopicsQuery(forumId, 0, 1);
        await _sut.Invoking(s => s.Execute(query, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
        

    }
    

    [Fact]
    public async Task ReturnTopics_ExtractedFromStorage_WhenForumExists()
    {
        var forumId = Guid.Parse("12C9B1ED-678D-4418-9DAE-5667FC528427");
        _getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = forumId } });
        
        var expectedResources = new Topic[] { new() };
        var expectedTotalCount = 6;

        _getTopicSetup.ReturnsAsync((expectedResources, expectedTotalCount));

        var (actualResources, actualTotalCount) = await _sut.Execute(
            new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedResources);
        actualTotalCount.Should().Be(expectedTotalCount);
        _storage.Verify(s => s.GetTopics(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
    }
}