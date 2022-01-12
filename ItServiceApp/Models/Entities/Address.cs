namespace ItServiceApp.Models.Entities
{
    public class Address : BaseEntity
    {
        public string Line { get; set; }
        public string PostCode { get; set; }
        public AddressTypes AddressType { get; set; }
    }

    public enum AddressTypes
    {
        Fatura,
        Teslimat
    }
}