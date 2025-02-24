using System.ComponentModel.DataAnnotations;

namespace PaymentService.Data.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }

        [Required]
        public string CardNumber { get; set; } = null!;
        [Required]
        public int CVV { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public decimal Amount { get; set; }

        public string Status { get; set; } = null!;
        public Guid OrderId { get; set; }
    }
}
