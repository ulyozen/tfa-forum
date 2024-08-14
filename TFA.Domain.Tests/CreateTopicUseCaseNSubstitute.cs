// using FluentAssertions;
// using NSubstitute;
// using TFA.Domain.Authentication;
// using TFA.Domain.Exceptions;
// using TFA.Domain.Models;
// using TFA.Domain.UseCases.CreateTopic;
//
// namespace TFA.Domain.Tests;
//
// public class CreateTopicUseCaseNSubstitute
// {
//     private readonly CreateTopicUseCase _sut;
//     private readonly ICreateTopicStorage _storage;
//     private readonly Task<Topic> _createTopicSetup;
//     private readonly Task<bool> _forumExistSetup;
//
//     private readonly IIdentityProvider _identityProvider;
//     private readonly IIdentity _identity;
//     
//     public CreateTopicUseCaseNSubstitute()
//     {
//         _storage = Substitute.For<ICreateTopicStorage>();
//         _forumExistSetup = _storage.ForumExist(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
//         _createTopicSetup = _storage.CreateTopic(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
//         
//         _identity = Substitute.For<IIdentity>();
//         _identityProvider = Substitute.For<IIdentityProvider>();
//         _identityProvider.Current.Returns(_identity);
//         
//         _sut = new CreateTopicUseCase(null, _storage, _identityProvider);
//     }
//     
//     [Fact]
//     public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
//     {
//         _forumExistSetup.Returns(false);
//         
//         var forumId = Guid.Parse("1C145780-E8E3-42E9-8145-417483A993F9");
//         
//         await _sut.Invoking(s => s.Execute(forumId, "Some title", CancellationToken.None))
//             .Should().ThrowAsync<ForumNotFoundException>();
//         
//         await _storage.Received(1).ForumExist(forumId, Arg.Any<CancellationToken>());
//     }
//     
//     [Fact]
//     public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExist()
//     {
//         var forumId = Guid.Parse("1C145780-E8E3-42E9-8145-417483A993F9");
//         var userId = Guid.Parse("2C6692CD-5785-42F5-902D-7403AD8F7B13");
//         
//         _identity.UserId.Returns(userId);
//         
//         _forumExistSetup.Returns(true);
//         
//
//         var expected = new Topic();
//         _createTopicSetup.Returns(expected); 
//         
//         var result = await _sut.Execute(forumId, "Hello world", CancellationToken.None);
//
//         result.Should().Be(expected);
//         
//         await _storage.Received(1).CreateTopic(forumId, userId, "Hello world", Arg.Any<CancellationToken>());
//     }
// }