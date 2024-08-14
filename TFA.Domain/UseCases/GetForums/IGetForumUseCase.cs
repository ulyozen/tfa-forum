using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetForums;

public interface IGetForumUseCase
{
    Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken);
}