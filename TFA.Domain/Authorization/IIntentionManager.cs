namespace TFA.Domain.Authorization;

public interface IIntentionManager
{
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;
    
    bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct;
}