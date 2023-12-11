namespace advance_csharp.dto.Request.Cart
{
    public class UpdateProductQuantityRequest
    {
        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }
    }
}