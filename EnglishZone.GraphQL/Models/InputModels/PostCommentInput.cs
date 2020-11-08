using System;
using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

namespace EnglishZone.GraphQL.Models.InputModels
{
    [InputType]
    public class PostCommentInput: OptionalWrapper
    {
        public string Owner { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }
    }
}