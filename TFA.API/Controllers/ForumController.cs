using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TFA.API.Models;
using TFA.Domain.Authorization;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using Topic = TFA.API.Models.Topic;

namespace TFA.API.Controllers;

[ApiController]
[Route("forums")]
public class ForumController : ControllerBase
{
    /// <summary>
    /// Get list of forums
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(Name = nameof(GetForums))]
    [ProducesResponseType(200, Type = typeof(ForumData[]))]
    public async Task<IActionResult> GetForums([FromServices] IGetForumUseCase useCase, CancellationToken cancellationToken)
    {
        var response = await useCase.Execute(cancellationToken);
        return Ok(response.Select(f => new ForumData
        {
            Id = f.Id,
            Title = f.Title
        }));
    }

    [HttpPost("{forumId:guid}/topics")]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(Topic))]
    public async Task<IActionResult> CreateTopic(
        Guid forumId,
        [FromBody] CreateTopic request,
        [FromServices] ICreateTopicUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new CreateTopicCommand(forumId, request.Title);
            var topic = await useCase.Execute(command, cancellationToken);
            return CreatedAtRoute(nameof(GetForums), new Topic()
            {
                Id = topic.Id,
                Title = topic.Title,
                CreatedAt = topic.CreateAt
            });
    }
}