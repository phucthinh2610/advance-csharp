

namespace advance_csharp.dto.Request.Cart
{
    public class CartRequest
    {
        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// List<CartDetailRequest> CartDetails
        /// </summary>
        public List<CartDetailRequest> CartDetails { get; set; } = new List<CartDetailRequest>();
    }
}