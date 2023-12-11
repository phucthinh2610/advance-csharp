namespace advance_csharp.dto.Response.Order
{
    public class GetAllOrderResponse
    {
        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Page Index
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Total Pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Total Order
        /// </summary>
        public long TotalOrder { get; set; }

        /// <summary>
        /// Orders
        /// </summary>
        public List<OrderResponse> Orders { get; set; } = new List<OrderResponse>();
    }
}