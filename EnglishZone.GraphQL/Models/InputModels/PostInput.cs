#region

using System;
using GraphQL.Conventions;
using GraphQL.Conventions.Attributes.Execution.Wrappers;

#endregion

namespace EnglishZone.GraphQL.Models.InputModels
{
    [InputType]
    public class PostInput : OptionalWrapper
    {
        public string Owner { get; set; }
        public string Content { get; set; }
    }
}