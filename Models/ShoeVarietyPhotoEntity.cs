using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mocassini.Models;

[Table("ShoeVarietyPhotos")]
public class ShoeVarietyPhotoEntity
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string ExternalPhotoId { get; set; } = null!;
    
    [Required]
    public int ShoeVarietyId { get; set; }
    public ShoeVarietyEntity? ShoeVariety { get; set; } = null!;
}