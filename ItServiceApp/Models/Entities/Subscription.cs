using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItServiceApp.Models.Entities
{
    public class Subscription : BaseEntity
    {
        public Guid SubscriptionTypeId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime EndDate { get; set; }

        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;

        [ForeignKey(nameof(SubscriptionTypeId))]
        public virtual SubscriptionType SubscriptionType { get; set; }
    }
}
