

namespace advance_csharp.dto.Response.Order
{
    public class UpdateOrderStatusResponse
    {
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// UpdatedOrder
        /// </summary>
        public OrderResponse UpdatedOrder { get; set; } = new OrderResponse();
    }
}