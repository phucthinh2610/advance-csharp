namespace advance_csharp.dto.Request.Cart
{
    public class CartDetailRequest
    {
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