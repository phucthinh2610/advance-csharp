using advance_csharp.dto.Request.Product;
using advance_csharp.dto.Response.Product;
using advance_csharp.service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace advance_csharp.Controllers
{
    /// <summary>
    ///  Controller Api product 
    /// </summary>
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// IProductService
        /// </summary>
        private readonly IProductService _productService;

        /// <summary>
        /// Product Controller
        /// </summary>
        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }
        /// <summary>
        /// get-product-user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("get-product-user")]
        [HttpGet()]
        [MyAppAuthentication("User")]
        public async Task<IActionResult> GetProduct([FromQuery] ProductGetListRequest request)
        {
            try
            {
                ProductGetListResponse response = await _productService.GetApplicationProductList(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                // send to logging service
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// get-product-admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("get-product-admin")]
        [HttpGet()]
        [MyAppAuthentication("Admin")]
        public async Task<IActionResult> GetProductAdmin([FromQuery] ProductGetListRequest request)
        {
            try
            {
                ProductGetListResponse response = await _productService.GetApplicationProductList(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                // send to logging service
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// create-product-admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("create-product-admin")]
        [HttpPost]
        [MyAppAuthentication("Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request)
        {
            try
            {
                ProductCreateResponse response = await _productService.CreateProduct(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// update-product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("update-product-admin")]
        [HttpPut]
        [MyAppAuthentication("Admin")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest request)
        {
            try
            {
                ProductUpdateResponse response = await _productService.UpdateProduct(request);

                return response.OldProduct == null || response.UpdatedProduct == null ? NotFound(response.Message) : Ok(response);
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete-product-admin")]
        [HttpDelete]
        [MyAppAuthentication("Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                ProductDeleteResponse response = await _productService.DeleteProduct(id);

                return response.DeletedProduct == null ? NotFound(response.Message) : Ok(response);
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