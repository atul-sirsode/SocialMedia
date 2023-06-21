using SocialMedia.Core.Queries;

namespace SocialMedia.Query.Api.Queries;

public class FindPostByAuthorQuery : BaseQuery
{
    public string Author { get; set; }
}