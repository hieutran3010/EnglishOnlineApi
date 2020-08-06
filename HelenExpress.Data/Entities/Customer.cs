#region

using EFPostgresEngagement.DataAnnotationAttributes;

#endregion

namespace HelenExpress.Data.Entities
{
    public class Customer : EntityBase
    {
        [UniqueIndex] public string Code { get; set; }

        [SimpleIndex] public string Name { get; set; }

        public string NickName { get; set; }

        [SimpleIndex] public string Phone { get; set; }

        [SimpleIndex] public string Address { get; set; }

        public string Hint { get; set; }
        public string SaleUserId { get; set; }

        public Bill[] SendBills { get; set; }
        public Bill[] ReceivedBills { get; set; }
    }
}