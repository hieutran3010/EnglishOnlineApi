using EFPostgresEngagement.DataAnnotationAttributes;

namespace EnglishZone.Data.Entities
{
    public class Post : EntityBase
    {
        [SimpleIndex] public string Owner { get; set; }
        public string Content { get; set; }
        public PostComment[] PostComments { get; set; }
    }
}