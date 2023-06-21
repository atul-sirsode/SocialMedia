using Microsoft.AspNetCore.Mvc;
using SocialMedia.Command.Api.Commands;
using SocialMedia.Command.Infrastructure.Exceptions;
using SocialMedia.Common.DTOs;
using SocialMedia.Core.Infrastructure;

namespace SocialMedia.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditPostController : ControllerBase
    {
        private readonly ILogger<EditPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public EditPostController(ILogger<EditPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> EditPostAsync(Guid Id, EditPostCommand command)
        {
            try
            {
                command.Id = Id;
                await _commandDispatcher.DispatchAsync(command);

                return StatusCode(StatusCodes.Status200OK, new BaseResponse
                {
                    Message = "Post edited successfully",
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
                const string serverError = "Error while processing a request to edit a post!";
                _logger.Log(LogLevel.Error, ex.Message, serverError);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = serverError,
                });
            }

        }
    }
}
