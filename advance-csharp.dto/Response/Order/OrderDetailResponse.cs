namespace advance_csharp.dto.Response.Order
{
    public class OrderDetailResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// OrderStatus
        /// </summary>
        public bool OrderStatus { get; set; }

        /// <summary>
        /// Order Status Description
        /// </summary>
        public string OrderStatusDescription { get; set; }

        public OrderDetailResponse()
        {
            // Default value for OrderStatusDescription
            OrderStatusDescription = "Chưa thanh toán";
        }
        /// <summary>
        /// SetOrderStatusDescription
        /// </summary>
        public void SetOrderStatusDescription()
        {
            OrderStatusDescription = OrderStatus ? "Đã thanh toán" : "Chưa thanh toán";
        }
    }
}