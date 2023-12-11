

namespace advance_csharp.dto.Response.Order
{
    public class OrderResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// OrderId
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// OrderDate
        /// </summary>
        public DateTimeOffset OrderDate { get; set; }

        /// <summary>
        /// TotalAmount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// IsDelete
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// Order details
        /// </summary>
        public List<OrderDetailResponse>? OrderDetails { get; set; }
    }
}