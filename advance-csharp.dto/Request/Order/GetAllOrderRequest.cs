namespace advance_csharp.dto.Request.Order
{
    public class GetAllOrderRequest : IPagingRequest
    {
        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Page Index
        /// </summary>
        public int PageIndex { get; set; } = 1;

    }
}