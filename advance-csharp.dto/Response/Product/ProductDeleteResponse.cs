namespace advance_csharp.dto.Response.Product
{
    /// <summary>
    /// Product delete response
    /// </summary>
    public class ProductDeleteResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Deleted Product
        /// </summary>
        public ProductResponse DeletedProduct { get; set; }

        /// <summary>
        /// Product delete response
        /// </summary>
        /// <param name="message"></param>
        /// <param name="deletedProduct"></param>
        public ProductDeleteResponse(string message, ProductResponse deletedProduct)
        {
            Message = message;
            DeletedProduct = deletedProduct;
        }

    }
}