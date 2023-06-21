using SocialMedia.Common.DTOs;
using SocialMedia.Query.Domain.Entities;

namespace SocialMedia.Query.Api.DTOs
{
    public class PostLookupResponse : BaseResponse
    {
        public List<PostEntity> Posts { get; set; }
    }
}
