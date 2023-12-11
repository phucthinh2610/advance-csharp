namespace advance_csharp.dto.Request.Cart
{
    public class DeleteProductFromCartRequest
    {
        /// <summary>
        /// UserId
        /// </summary>
        public Guid CartId { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        public Guid ProductId { get; set; }
    }
}