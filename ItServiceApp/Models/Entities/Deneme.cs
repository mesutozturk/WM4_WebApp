using System.ComponentModel.DataAnnotations;

namespace ItServiceApp.Models.Entities
{
    public class Deneme
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Ad { get; set; }
    }
}