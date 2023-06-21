using Microsoft.AspNetCore.Mvc;
using SocialMedia.Command.Api.Commands;
using SocialMedia.Command.Infrastructure.Exceptions;
using SocialMedia.Common.DTOs;
using SocialMedia.Core.Infrastructure;

namespace SocialMedia.Command.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LikedPostController : ControllerBase
    {
        private readonly ILogger<LikedPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public LikedPostController(ILogger<LikedPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> LikePostAsync(Guid id)
        {
            try
            {
                
                await _commandDispatcher.DispatchAsync(new LikePostCommand
                {
                    Id = id
                });

                return StatusCode(StatusCodes.Status200OK, new BaseResponse
                {
                    Message = "Post Like successfully",
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
                const string serverError = "Error while processing a request to like a post!";
                _logger.Log(LogLevel.Error, ex.Message, serverError);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = serverError,
                });
            }

        }
    }
}
