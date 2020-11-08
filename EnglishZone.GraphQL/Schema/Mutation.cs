#region

using EnglishZone.Data.Entities;
using EnglishZone.GraphQL.Models.InputModels;
using GraphQL.Conventions;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;

#endregion

namespace EnglishZone.GraphQL.Schema
{
    internal sealed class Mutation : IMutation
    {
        public EntityMutationBase<Post, PostInput> Post(
            [Inject] EntityMutationBase<Post, PostInput> entityMutation)
        {
            return entityMutation;
        }

        public EntityMutationBase<PostComment, PostCommentInput> PostComment(
            [Inject] EntityMutationBase<PostComment, PostCommentInput> entityMutation)
        {
            return entityMutation;
        }
    }
}