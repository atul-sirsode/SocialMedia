using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Query.Domain.Entities
{
    [Table("Post")]
    public class PostEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
        public DateTime DatePosted { get; set; }
        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}
