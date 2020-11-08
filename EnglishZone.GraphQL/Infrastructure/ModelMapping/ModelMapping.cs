using EnglishZone.Data.Entities;
using EnglishZone.GraphQL.Models.InputModels;
using Mapster;

namespace EnglishZone.GraphQL.Infrastructure.ModelMapping
{
    public class ModelMapping
    {
        public static void Mapping()
        {
            var config = new TypeAdapterConfig();
            config.ForType<PostInput, Post>();
        }
    }
}