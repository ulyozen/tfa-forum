using TFA.Domain.Authentication;

namespace TFA.Domain.Authorization;

public interface IIntentionResolver;

// TODO: Почитать для чего используется понятие Ковариантность [https://metanit.com/sharp/tutorial/3.27.php]
public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention);
}