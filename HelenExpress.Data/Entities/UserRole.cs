#region

using EFPostgresEngagement.DataAnnotationAttributes;

#endregion

namespace HelenExpress.Data.Entities
{
    public class UserRole : EntityBase
    {
        [UniqueIndex(AdditionalColumns = "Role")]
        public string UserId { get; set; }

        [SimpleIndex] public string Role { get; set; }
    }
}