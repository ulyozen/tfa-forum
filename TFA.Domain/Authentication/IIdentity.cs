namespace TFA.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
}

internal class User(Guid userId) : IIdentity
{
    public Guid UserId { get; } = userId;
}

internal static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}