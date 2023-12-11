

namespace advance_csharp.dto.Response.Cart
{
    public class CartResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Cart detail
        /// </summary>
        public List<CartDetailResponse>? CartDetails { get; set; }
    }
}