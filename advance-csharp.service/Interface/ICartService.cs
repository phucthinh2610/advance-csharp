using advance_csharp.dto.Request.Cart;
using advance_csharp.dto.Response.Cart;

namespace advance_csharp.service.Interface
{
    public interface ICartService
    {
        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        Task<GetAllCartResponse> GetAllCarts(GetAllCartRequest request);

        /// <summary>
        /// Get Cart By User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CartResponse> GetCartByUserId(GetCartByUserIdRequest request);

        /// <summary>
        /// AddProductToCart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AddProductToCartResponse> AddProductToCart(CartRequest request);

        /// <summary>
        /// DeleteProductFromCart
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<CartResponse> DeleteProductFromCart(DeleteProductFromCartRequest request);

        /// <summary>
        /// UpdateQuantity
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<bool> UpdateQuantity(UpdateProductQuantityRequest request);
    }

}