namespace advance_csharp.dto.Request.Product
{
    /// <summary>
    /// Product update request
    /// </summary>
    public class ProductUpdateRequest
    {
        /// <summary>
        /// Product Id
        /// </summary>
        public Guid Id { get; set; }

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
        public int Quantity { get; set; }

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

        /// <summary>
        /// Check Price is Number
        /// </summary>
        public bool IsPriceValid => decimal.TryParse(Price, out _);
    }
}