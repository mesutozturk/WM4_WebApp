using ItServiceApp.Core.Entities.Abstracts;
using ItServiceApp.Core.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItServiceApp.Core.Entities
{
    public class Address : BaseEntity<Guid>
    {
        public string Line { get; set; }
        public string PostCode { get; set; }
        public AddressTypes AddressType { get; set; }
        public int StateId { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }

    public enum AddressTypes
    {
        Fatura,
        Teslimat
    }
}