
using System.ComponentModel.DataAnnotations.Schema;

namespace advance_csharp.database.Models
{
    /// <summary>
    /// Table Product
    /// </summary>
    [Table("product")]
    public class Product : BaseEntity
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Price
        /// </summary>
        public string Price { get; set; } = string.Empty;

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// Unit
        /// </summary>
        public string Unit { get; set; } = "VND";

        /// <summary>
        /// Images
        /// </summary>
        public string Images { get; set; } = string.Empty;

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; } = string.Empty;

    }
}