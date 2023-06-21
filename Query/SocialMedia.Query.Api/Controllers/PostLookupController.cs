using Microsoft.AspNetCore.Mvc;
using SocialMedia.Common.DTOs;
using SocialMedia.Core.Infrastructure;
using SocialMedia.Query.Api.DTOs;
using SocialMedia.Query.Api.Queries;
using SocialMedia.Query.Domain.Entities;

namespace SocialMedia.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindAllPostQuery());
                return SuccessResponse(posts);
            }
            catch (Exception ex)
            {
                return FailureResponse(ex, "all post");
            }
        }

        [HttpGet("byId/{postId}")]
        public async Task<IActionResult> GetPostByIdAsync(Guid postId)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });
                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully return post!"
                });
            }
            catch (Exception ex)
            {
                return FailureResponse(ex, "post by Id");
               
            }
        }
        [HttpGet("byAuthor/{author}")]
        public async Task<IActionResult> GetPostByAuthorAsync(string author)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByAuthorQuery { Author = author });
                return SuccessResponse(posts);
            }
            catch (Exception ex)
            {
                return FailureResponse(ex, "post by Author");
            }
        }

        [HttpGet("withComments")]
        public async Task<IActionResult> GetPostWithCommentAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostWithCommentsQuery());
                return SuccessResponse(posts);
            }
            catch (Exception ex)
            {
                return FailureResponse(ex ,"all Comments");
            }
        }
        [HttpGet("withLikes/{numberOfLikes}")]
        public async Task<IActionResult> GetPostWithLikesAsync(int numberOfLikes)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostWithLikesQuery{NumberOfLikes = numberOfLikes});
                return SuccessResponse(posts);
            }
            catch (Exception ex)
            {
                return FailureResponse(ex, "post by likes");
            }
        }

        private ActionResult SuccessResponse(List<PostEntity> posts)
        {
            if (posts == null || !posts.Any())
            {
                return NoContent();
            }

            var postCount = posts.Count;
            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Successfully return {postCount} post{(postCount > 1 ? "s" : string.Empty)}!"
            });
        }

        private ActionResult FailureResponse(Exception ex , string message)
        {
             var serverError = $"Error while processing a request to get {message} !";
            _logger.Log(LogLevel.Error, ex.Message, serverError);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = serverError,
            });

        }
    }
}
