using EFPostgresEngagement.Abstract;

namespace Api.GraphQL
{
    public class ApplicationUserProvider: IDbTracker
    {
        public string GetAuthor()
        {
            return "System";
        }
    }
}