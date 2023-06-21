using Microsoft.AspNetCore.Mvc;
using SocialMedia.Command.Api.Commands;
using SocialMedia.Command.Infrastructure.Exceptions;
using SocialMedia.Common.DTOs;
using SocialMedia.Core.Infrastructure;

namespace SocialMedia.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AddCommentController : ControllerBase
    {
        private readonly ILogger<AddCommentController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public AddCommentController(ILogger<AddCommentController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddCommentAsync(Guid id, AddCommentCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.DispatchAsync(command);

                return StatusCode(StatusCodes.Status200OK, new BaseResponse
                {
                    Message = "Add comment request completed successfully",
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
                _logger.Log(LogLevel.Warning, "Could not retrieve aggregate, client passed an incorrect post id targeting the aggregate !");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                const string serverError = "Error while processing a request to add a comment to a post!";
                _logger.Log(LogLevel.Error, ex.Message, serverError);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = serverError,
                });
            }

        }
    }
}
