using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mocassini.Models;

[Table("OrderItems")]
public class OrderItemEntity
{
    [Key] public int Id { get; set; }

    [Required] public int ShoeVarietyId { get; set; }
    public ShoeVarietyEntity? ShoeVariety { get; set; } = null!;

    [Required] public int OrderId { get; set; }
    public OrderEntity? Order { get; set; } = null!;
}