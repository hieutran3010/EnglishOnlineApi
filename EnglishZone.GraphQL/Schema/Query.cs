#region

using EnglishZone.Data.Entities;
using GraphQL.Conventions;
using GraphQLDoorNet;
using GraphQLDoorNet.Abstracts;

#endregion

namespace EnglishZone.GraphQL.Schema
{
    internal sealed class Query : IQuery
    {
        public EntityQueryBase<Post> Post(
            [Inject] EntityQueryBase<Post> entityQuery)
        {
            return entityQuery;
        }
        
        public EntityQueryBase<PostComment> PostComment(
            [Inject] EntityQueryBase<PostComment> entityQuery)
        {
            return entityQuery;
        }
    }
}