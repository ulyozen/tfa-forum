using FluentAssertions;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.Tests;

public class CreateTopicCommandValidatorShould
{
    private readonly CreateTopicCommandValidator sut;

    public CreateTopicCommandValidatorShould()
    {
        sut = new CreateTopicCommandValidator();
    }

    [Fact]
    public void ReturnSuccess_WhenCommandIsValid()
    {
        var actual = sut.Validate(new CreateTopicCommand(Guid.Parse("D24E82C6-C923-40DF-B7E3-FE4256F7EA25"), "Hello"));
        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var validCommand = new CreateTopicCommand(Guid.Parse("B017E2DC-512B-4BB3-AC5F-7F91A6DE954E"), "Hello");
        yield return new object[] { validCommand with{ ForumId = Guid.Empty} };
        yield return new object[] { validCommand with { Title = string.Empty } };
        yield return new object[] { validCommand with { Title = "   " } };
        yield return new object[] { validCommand with { Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc eget leo ac lectus ultricies ullamcorper. Etiam lobortis, augue faucibus tristique." } };
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WhenCommandIsInvalid(CreateTopicCommand command)
    {
        var actual = sut.Validate(command);
        actual.IsValid.Should().BeFalse();
    }
}