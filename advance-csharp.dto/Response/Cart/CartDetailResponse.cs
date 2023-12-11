namespace advance_csharp.dto.Response.Cart
{
    public class CartDetailResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// CartId
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
    }
}