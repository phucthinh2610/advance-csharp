namespace advance_csharp.dto.Request.Product
{
    public class ProductGetListRequest : IPagingRequest
    {
        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Page Index
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// Search: Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Search: Category
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Search: PriceFrom
        /// </summary>
        public string PriceFrom { get; set; } = string.Empty;

        /// <summary>
        /// Search: PriceFrom
        /// </summary>
        public string PriceTo { get; set; } = string.Empty;


    }
}