using System.ComponentModel.DataAnnotations;

namespace PaymentService.Dtos
{
    public class PaymentDto
    {
        [Required]
        public string CardNumber { get; set; } = null!;
        [Required]
        public int CVV { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public decimal Amount { get; set; }

    }
}
