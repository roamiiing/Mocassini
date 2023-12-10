using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mocassini.Utils;

namespace Mocassini.Models;

[Table("ShoeVarieties")]
public class ShoeVarietyEntity
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [RegularExpression(
        ValidationUtils.AllowedRegex,
        ErrorMessage = ValidationUtils.AllowedRegexMessage
    )]
    public string Name { get; set; } = null!;

    [Required] public int ShoeId { get; set; }
    public ShoeEntity? Shoe { get; set; }
 
    [Required]
    public int Quantity { get; set; } = 0;
    
    public IEnumerable<OrderItemEntity>? OrderItems { get; set; } = null!;
    
    public IEnumerable<ShoeVarietyPhotoEntity>? ShoeVarietyPhotos { get; set; } = null!;
}