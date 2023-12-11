namespace advance_csharp.dto.Response.Cart
{
    public class AddProductToCartResponse
    {
        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message in case of failure
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Cart details after the product is added
        /// </summary>
        public CartResponse? UpdatedCart { get; set; }
    }
}