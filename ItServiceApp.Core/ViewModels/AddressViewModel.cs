using System;
using ItServiceApp.Core.Entities;

namespace ItServiceApp.ViewModels
{
    public class AddressViewModel
    {
        public Guid Id { get; set; }
        public string Line { get; set; }
        public string PostCode { get; set; }
        public AddressTypes AddressType { get; set; }
        public int StateId { get; set; }
        public string UserId { get; set; }
    }
}
