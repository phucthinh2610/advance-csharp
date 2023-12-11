namespace advance_csharp.dto.Response.Product
{
    /// <summary>
    /// Product Create Response
    /// </summary>
    public class ProductCreateResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// productResponse
        /// </summary>
        public ProductResponse? productResponse { get; set; }
    }
}