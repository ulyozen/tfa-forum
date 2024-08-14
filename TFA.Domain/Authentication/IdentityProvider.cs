namespace TFA.Domain.Authentication;

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; } = new User(Guid.Parse("4576F086-5603-424A-A251-1FBF13F47C05"));
}