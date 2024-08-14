using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetForums;

public interface IGetForumStorage
{
    Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken);
}