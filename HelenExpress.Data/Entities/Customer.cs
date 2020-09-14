#region

using System.Collections.Generic;
using EFPostgresEngagement.DataAnnotationAttributes;

#endregion

namespace HelenExpress.Data.Entities
{
    public class Customer : EntityBase
    {
        [UniqueIndex] public string Code { get; set; }

        [SimpleIndex] public string Name { get; set; }
        [SimpleIndex] public string NameNonUnicode { get; set; }

        [UniqueIndex] public string Phone { get; set; }

        [SimpleIndex] public string Address { get; set; }

        public string Hint { get; set; }

        public List<Bill> SendBills { get; set; }
        public List<Bill> ReceivedBills { get; set; }
    }
}