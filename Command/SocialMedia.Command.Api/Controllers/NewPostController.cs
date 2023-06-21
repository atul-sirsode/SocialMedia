using Microsoft.AspNetCore.Mvc;
using SocialMedia.Command.Api.Commands;
using SocialMedia.Common.DTOs;
using SocialMedia.Core.Infrastructure;

namespace SocialMedia.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewPostController : ControllerBase
    {
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> NewPostAsync(NewPostCommand command)
        {
            try
            {
                var id = Guid.NewGuid();
                command.Id = id;
                await _commandDispatcher.DispatchAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Id = id,
                    Message = "New post created successfully",
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, "Client made a bad request!");
                return BadRequest(new NewPostResponse
                {
                    Message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                const string serverError = "Error while processing a request to create a new post!";
                _logger.Log(LogLevel.Error, ex.Message, serverError);
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Message = serverError,
                });
            }

        }
    }
}
