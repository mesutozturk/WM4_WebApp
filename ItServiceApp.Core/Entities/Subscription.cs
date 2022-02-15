using ItServiceApp.Core.Entities.Abstracts;
using ItServiceApp.Core.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItServiceApp.Core.Entities
{
    public class Subscription : BaseEntity<Guid>
    {
        public Guid SubscriptionTypeId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime EndDate { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;

        [ForeignKey(nameof(SubscriptionTypeId))]
        public virtual SubscriptionType SubscriptionType { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }
}
