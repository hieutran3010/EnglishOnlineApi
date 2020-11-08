using System;
using EFPostgresEngagement.DataAnnotationAttributes;

namespace EnglishZone.Data.Entities
{
    public class PostComment: EntityBase
    {
        [SimpleIndex]
        public string Owner { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}