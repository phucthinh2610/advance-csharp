using advance_csharp.dto.Request.Cart;
using advance_csharp.dto.Response.Cart;
using advance_csharp.service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace advance_csharp.Controllers
{
    /// <summary>
    /// CartController
    /// </summary>
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        /// <summary>
        /// ICartService
        /// </summary>
        private readonly ICartService _cartService;

        /// <summary>
        ///  CartController(ICartService cartService)
        /// </summary>
        /// <param name="cartService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CartController(ICartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        /// <summary>
        /// view-all-cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("/view-all-cart")]
        [HttpGet()]
        public async Task<IActionResult> GetAllCarts([FromQuery] GetAllCartRequest request)
        {
            try
            {
                GetAllCartResponse response = await _cartService.GetAllCarts(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// ViewCart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("/view-cart")]
        [HttpGet()]
        public async Task<IActionResult> GetCartByUserId([FromQuery] Guid userId)
        {
            try
            {
                GetCartByUserIdRequest request = new() { UserId = userId };

                CartResponse cartResponse = await _cartService.GetCartByUserId(request);

                return cartResponse != null ? Ok(cartResponse) : BadRequest("Failed to get cart for the specified user.");
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// AddProductToCart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("/add-product")]
        [HttpPost()]
        public async Task<IActionResult> AddProductToCart([FromBody] CartRequest request)
        {
            try
            {
                AddProductToCartResponse response = await _cartService.AddProductToCart(request);

                return response.IsSuccess
                    ? Ok(new { Message = "Product added to the cart successfully.", response.UpdatedCart })
                    : BadRequest(new { ErrorMessage = "Failed to add the product to the cart.", DetailedError = response.ErrorMessage });
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// DeleteProductFromCart
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [Route("/delete-product")]
        [HttpDelete()]
        public async Task<IActionResult> DeleteProductFromCart([FromBody] DeleteProductFromCartRequest request)
        {
            try
            {
                CartResponse result = await _cartService.DeleteProductFromCart(request);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    // Handle the case where the request is invalid or product not found
                    return NotFound(new { Message = "Invalid request or product not found" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        /// <summary>
        /// UpdateQuantity
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [Route("api/update-quantity")]
        [HttpPut()]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateProductQuantityRequest request)
        {
            try
            {
                bool result = await _cartService.UpdateQuantity(request);

                return result
                    ? Ok("Cart quantity updated successfully.")
                    : BadRequest("Failed to update cart quantity.");
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}