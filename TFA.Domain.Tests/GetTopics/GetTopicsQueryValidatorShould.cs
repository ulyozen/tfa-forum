using FluentAssertions;
using FluentValidation;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Domain.Tests.GetTopics;

public class GetTopicsQueryValidatorShould
{
    private readonly GetTopicsQueryValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenQueryIsValid()
    {
        var query = new GetTopicsQuery(Guid.Parse("9CE8D464-19BE-4C66-905B-5B2D2751CA20"), 10, 5);
        sut.Validate(query).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidQuery()
    {
        var query = new GetTopicsQuery(Guid.Parse("9CE8D464-19BE-4C66-905B-5B2D2751CA20"), 10, 5);
        yield return new object[] { query with { ForumId = Guid.Empty} };
        yield return new object[] { query with { Skip = -40 } };
        yield return new object[] { query with { Take = -1 } };
    }

    [Theory]
    [MemberData(nameof(GetInvalidQuery))]
    public void ReturnFailure_WhenQueryIsInvalid(GetTopicsQuery query)
    {
        sut.Validate(query).IsValid.Should().BeFalse();
    }
}