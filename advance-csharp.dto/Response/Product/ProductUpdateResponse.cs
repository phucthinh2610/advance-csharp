

namespace advance_csharp.dto.Response.Product
{
    /// <summary>
    /// Product Update Response
    /// </summary>
    public class ProductUpdateResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Old Product
        /// </summary>
        public ProductResponse? OldProduct { get; set; }

        /// <summary>
        /// Updated Product
        /// </summary>
        public ProductResponse? UpdatedProduct { get; set; }
    }
}