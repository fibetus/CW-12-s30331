using System.ComponentModel.DataAnnotations;

namespace CW12_s30331.DTOs.TripDTOs;

public class TripClientCreateDto
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(120)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(120)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(120)]
    [Phone]
    public string Telephone { get; set; }

    [Required]
    [MaxLength(120)]
    [RegularExpression("^[0-9]{11}$", ErrorMessage = "PESEL must be 11 digits.")]
    public string Pesel { get; set; }
    
    public DateTime? PaymentDate { get; set; }
}