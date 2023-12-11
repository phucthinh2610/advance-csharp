
using System.ComponentModel.DataAnnotations.Schema;

namespace advance_csharp.database.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }

        /// <summary>
        /// OrderId
        /// </summary>
        [ForeignKey("OrderId")]
        public Guid OrderId { get; set; }

        /// <summary>
        /// OrderStatus
        /// </summary>
        public bool OrderStatus { get; set; } = false;

        /// <summary>
        /// Navigation property to the related Product
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Navigation property to the related Order
        /// </summary>
        public Order? Order { get; set; }
    }
}