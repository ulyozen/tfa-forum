namespace TFA.Domain.Authorization;

public interface IIntentionManager
{
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;
    
    bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct;
}

public static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention) 
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention)) throw new IntentionManagerException();
    }
}