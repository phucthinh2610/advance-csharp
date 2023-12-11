
using System.ComponentModel.DataAnnotations.Schema;

namespace advance_csharp.database.Models
{
    /// <summary>
    /// Cart detail
    /// </summary>
    [Table("cartDetail")]
    public class CartDetail
    {
        /// <summary>
        /// Id cart detail
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Cart id
        /// </summary>
        public Guid CartId { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public string Price { get; set; } = string.Empty;

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// IsDelete
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// CartId is ForeignKey
        /// </summary>
        [ForeignKey("CartId")]
        public Cart? Cart { get; set; }

        /// <summary>
        /// Product is ForeignKey
        /// </summary>
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }

}