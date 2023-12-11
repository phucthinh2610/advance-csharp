using advance_csharp.service.Interface;
using advance_csharp.database;
using advance_csharp.database.Models;
using advance_csharp.dto.Request.Cart;
using advance_csharp.dto.Response.Cart;
using advance_csharp.service.Interface;
using Microsoft.EntityFrameworkCore;

namespace advance_csharp.service.Service
{
    public class CartService : ICartService
    {
        private readonly DbContextOptions<AdvanceCsharpContext> _context;
        private readonly IProductService _productService;

        public CartService(DbContextOptions<AdvanceCsharpContext> dbContextOptions, IProductService productService)
        {
            _context = dbContextOptions;
            _productService = productService;
        }

        /// <summary>
        /// GetAllCarts
        /// </summary>
        /// <returns></returns>
        public async Task<GetAllCartResponse> GetAllCarts(GetAllCartRequest request)
        {
            GetAllCartResponse response = new()
            {
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };

            try
            {
                using AdvanceCsharpContext context = new(_context);

                IQueryable<Cart> query = context.Carts.Include(c => c.CartDetails);

                // Count the total number of carts according to filtered conditions
                response.TotalCarts = await query.CountAsync();

                // Calculate the number of pages and total pages
                response.TotalPages = (int)Math.Ceiling((double)response.TotalCarts / request.PageSize);

                // Perform pagination and get data for the current page
                int startIndex = (request.PageIndex - 1) * request.PageSize;
                int endIndex = startIndex + request.PageSize;
                query = query.Skip(startIndex).Take(request.PageSize);

                List<Cart> allCarts = await query.ToListAsync();

                response.Carts = allCarts.Select(cart => new CartResponse
                {
                    Message = "Success! The user's cart has a UserId of: " + cart.UserId,
                    Id = cart.Id,
                    UserId = cart.UserId,
                    CartDetails = cart.CartDetails?.Select(cd => new CartDetailResponse
                    {
                        Id = cd.Id,
                        CartId = cd.CartId,
                        ProductId = cd.ProductId,
                        Price = cd.Price,
                        Quantity = cd.Quantity,
                        IsDelete = cd.IsDelete,
                    }).ToList() ?? new List<CartDetailResponse>()
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                Console.WriteLine($"An error occurred while retrieving all carts: {ex.Message}");
                response.Message = "Error retrieving carts";
                return response;
            }
        }

        /// <summary>
        /// GetCartByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CartResponse> GetCartByUserId(GetCartByUserIdRequest request)
        {
            try
            {
                if (request == null)
                {
                    // Handle the case where the request is null
                    return new CartResponse
                    {
                        Message = "Error: GetCartByUserIdRequest is null."
                    };
                }

                using AdvanceCsharpContext context = new(_context);

                if (context.Carts == null)
                {
                    // Handle the case where context.Carts is null
                    return new CartResponse
                    {
                        Message = "Error: context.Carts is null."
                    };
                }

                Cart? cart = await context.Carts
                    .Include(c => c.CartDetails)
                    .FirstOrDefaultAsync(c => c.UserId == request.UserId);

                if (cart == null)
                {
                    // Create an empty cart if it doesn't exist
                    cart = new Cart { UserId = request.UserId };
                    _ = context.Carts.Add(cart);
                    _ = await context.SaveChangesAsync();
                }

                CartResponse cartResponse = new()
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    CartDetails = cart.CartDetails?.Select(cd => new CartDetailResponse
                    {
                        Id = cd.Id,
                        CartId = cd.CartId,
                        ProductId = cd.ProductId,
                        Price = cd.Price,
                        Quantity = cd.Quantity,
                        IsDelete = cd.IsDelete,
                    }).ToList() ?? new List<CartDetailResponse>()
                };

                return cartResponse;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                return new CartResponse
                {
                    Message = $"An error occurred while retrieving the cart: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// AddProductToCart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AddProductToCartResponse> AddProductToCart(CartRequest request)
        {
            AddProductToCartResponse addToCartResponse = new();

            try
            {
                using AdvanceCsharpContext context = new(_context);

                // Check if the user's cart already exists
                Cart? cart = await context.Carts
                    .Include(c => c.CartDetails)
                    .FirstOrDefaultAsync(c => c.UserId == request.UserId);

                // If the cart doesn't exist, create a new cart
                cart ??= new Cart
                {
                    UserId = request.UserId,
                    CartDetails = new List<CartDetail>()
                };

                // Check if the product already exists in the cart
                CartDetail? existingCartDetail = cart?.CartDetails?
                    .FirstOrDefault(cd => cd.ProductId == request.CartDetails?[0]?.ProductId);

                // Check if the product doesn't exist or if CartDetails is null
                if (existingCartDetail != null)
                {
                    // Check if the product is marked as deleted
                    if (await IsProductDeletedAsync(existingCartDetail.ProductId))
                    {
                        // Both existing and new products are marked as deleted
                        addToCartResponse.IsSuccess = false;
                        addToCartResponse.ErrorMessage = "Cannot add the product to the cart. Product is marked as deleted.";
                        return addToCartResponse;
                    }
                    else
                    {
                        // If the existing product is not marked as deleted, update the quantity
                        existingCartDetail.Quantity += request.CartDetails?[0]?.Quantity ?? 0;
                    }
                }
                else if (cart?.CartDetails != null)
                {
                    // If the product doesn't exist, add a new cart detail
                    Guid productId = request.CartDetails?[0]?.ProductId ?? Guid.Empty;
                    bool isProductDeleted = await IsProductDeletedAsync(productId);

                    if (!isProductDeleted)
                    {
                        CartDetail newCartDetail = new()
                        {
                            ProductId = productId,
                            Quantity = request.CartDetails?[0]?.Quantity ?? 0,
                            // Set the Price based on the product's price
                            Price = await GetProductPriceAsync(productId)
                        };

                        cart.CartDetails.Add(newCartDetail);
                    }
                    else
                    {
                        // Product is marked as deleted
                        addToCartResponse.IsSuccess = false;
                        addToCartResponse.ErrorMessage = "Cannot add the product to the cart. Product is marked as deleted.";
                        return addToCartResponse;
                    }
                }

                // Update or add the cart to the database
                if (cart != null)
                {
                    if (cart.Id == Guid.Empty)
                    {
                        // If it's a new cart or doesn't exist in the database, add it
                        _ = context.Carts.Add(cart);
                    }
                    else
                    {
                        // If the cart already exists, update it in the database
                        _ = context.Carts.Update(cart);
                    }
                }

                // Save changes to the database
                _ = await context.SaveChangesAsync();

                // Prepare the response
                addToCartResponse.IsSuccess = true;
                addToCartResponse.UpdatedCart = await GetCartByUserId(new GetCartByUserIdRequest { UserId = request.UserId });
            }
            catch (Exception ex)
            {
                // Handle exceptions and log errors
                Console.WriteLine($"Error in AddProductToCart: {ex.Message}");
                addToCartResponse.IsSuccess = false;
                addToCartResponse.ErrorMessage = $"An error occurred: {ex.Message}";
            }

            return addToCartResponse;
        }

        /// <summary>
        /// DeleteProductFromCart
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<CartResponse> DeleteProductFromCart(DeleteProductFromCartRequest request)
        {
            try
            {
                if (request == null || request.CartId == Guid.Empty || request.ProductId == Guid.Empty)
                {
                    // Handle the case where the request is invalid
                    return new CartResponse { Message = "Invalid request" };
                }

                using AdvanceCsharpContext context = new(_context);
                Cart? cart = await context.Carts
                    .Include(c => c.CartDetails)
                    .FirstOrDefaultAsync(c => c.UserId == request.CartId);

                if (cart != null && cart.CartDetails != null)
                {
                    // Find the cart items to remove
                    List<CartDetail> cartItemsToRemove = cart.CartDetails
                        .Where(cd => cd.ProductId == request.ProductId)
                        .ToList();

                    if (cartItemsToRemove.Any())
                    {
                        // Log the values for debugging
                        Console.WriteLine($"Deleting product with ProductId: {request.ProductId} from CartId: {request.CartId}");

                        // Remove the cart items from the context
                        context.CartDetails?.RemoveRange(cartItemsToRemove);
                        _ = await context.SaveChangesAsync();

                        // Return the updated CartResponse after successful deletion
                        return new CartResponse
                        {
                            Message = "Product deleted from cart successfully",
                            Id = cart.Id,
                            UserId = cart.UserId,
                            CartDetails = cart.CartDetails.Select(cd => new CartDetailResponse
                            {
                                Id = cd.Id,
                                CartId = cd.CartId,
                                ProductId = cd.ProductId,
                                Price = cd.Price,
                                Quantity = cd.Quantity,
                                IsDelete = cd.IsDelete
                            }).ToList()
                        };
                    }
                    else
                    {
                        // Log the values for debugging
                        Console.WriteLine($"Product with ProductId: {request.ProductId} not found in CartId: {request.CartId}");

                        // Handle the case where the product was not found in the cart
                        return new CartResponse { Message = "Product not found in the cart" };
                    }
                }
                else
                {
                    // Log the values for debugging
                    Console.WriteLine($"CartId: {request.CartId}, ProductId: {request.ProductId}");

                    // Handle the case where the cart or cartDetails is null
                    return new CartResponse { Message = "Cart not found or cart details not available" };
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error in DeleteProductFromCart: {ex.Message}");

                // Handle exceptions, log, or rethrow
                return new CartResponse { Message = "Error deleting product from cart" };
            }
        }

        /// <summary>
        /// UpdateQuantity
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateQuantity(UpdateProductQuantityRequest request)
        {
            try
            {
                if (request == null)
                {
                    // Handle the case where the request is null
                    return false;
                }

                using AdvanceCsharpContext context = new(_context);

                Cart? cart = await context.Carts
                    .Include(c => c.CartDetails)
                    .FirstOrDefaultAsync(c => c.UserId == request.UserId);

                if (cart != null)
                {
                    CartDetail? cartItem = cart.CartDetails?.FirstOrDefault(cd => cd.ProductId == request.ProductId);

                    if (cartItem != null)
                    {
                        cartItem.Quantity = request.Quantity;
                        _ = await context.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                Console.WriteLine($"An error occurred while updating the product quantity: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// IsProductDeletedAsync
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private async Task<bool> IsProductDeletedAsync(Guid productId)
        {
            using AdvanceCsharpContext context = new(_context);

            // Check if the context.Products is not null
            if (context.Products != null)
            {
                // Check if the product with the given Id is marked as deleted
                return await context.Products
                    .Where(p => p.Id == productId && p.IsDelete)
                    .AnyAsync();
            }
            else
            {
                // Handle the case where context.Products is null
                Console.WriteLine("Error: context.Products is null");
                return false;
            }
        }

        /// <summary>
        /// GetProductPriceAsync
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private async Task<string> GetProductPriceAsync(Guid productId)
        {
            try
            {
                // Use the injected ProductService to get the product price
                string productPrice = await _productService.GetProductPriceById(productId);
                return productPrice;
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
                Console.WriteLine($"Error while getting product price by ID {productId}: {ex.Message}");
                return string.Empty; // Return an empty string or handle the error case accordingly
            }
        }

    }
}