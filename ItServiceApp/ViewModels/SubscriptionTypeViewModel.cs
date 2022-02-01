using System;

namespace ItServiceApp.ViewModels
{
    public class SubscriptionTypeViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Month { get; set; }
        public decimal Price { get; set; }
    }
}
