using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Infrastructure;
using SocialMedia.Command.Api.Commands;
using SocialMedia.Command.Infrastructure.Exceptions;
using SocialMedia.Common.DTOs;

namespace SocialMedia.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RemovePostController : ControllerBase
    {
        private readonly ILogger<RemovePostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RemovePostController(ILogger<RemovePostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePostAsync(Guid id, RemovePostCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.DispatchAsync(command);

                return StatusCode(StatusCodes.Status200OK, new BaseResponse
                {
                    Message = "post removed successfully",
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, "Client made a bad request!");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message,
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning,
                    "Could not retrieve aggregate, client passed an incorrect comment id targeting the aggregate !");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                const string serverError = "Error while processing a request to remove a post!";
                _logger.Log(LogLevel.Error, ex.Message, serverError);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = serverError,
                });
            }
        }
    }
}
