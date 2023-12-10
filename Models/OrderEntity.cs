using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mocassini.Models;

[Table("Orders")]
public class OrderEntity
{
    [Key] public int Id { get; set; }

    [Required] public AddressVO Address { get; set; } = null!;

    [Required] public int UserId { get; set; }
    public UserEntity? User { get; set; }

    public IEnumerable<OrderItemEntity>? Items { get; set; }
}