

namespace advance_csharp.dto.Response.Cart
{
    public class GetAllCartResponse
    {
        public string Message { get; set; } = string.Empty;

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

        public int TotalCarts { get; set; }


        /// <summary>
        /// Cart detail
        /// </summary>
        public List<CartResponse>? Carts { get; set; }
    }
}