using GraphQLDoorNet.Abstracts;
using Mapster;

namespace EnglishZone.GraphQL.Infrastructure.ModelMapping
{
    public class InputMapper: IInputMapper
    {
        public TDest Map<TSource, TDest>(TSource source)
        {
            return source.Adapt<TDest>();
        }

        public void MapUpdate<TSource, TDest>(TSource source, TDest dest)
        {
            source.Adapt(dest);
        }
    }
}