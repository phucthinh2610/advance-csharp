using advance_csharp.service.Interface;
using advance_csharp.database;
using advance_csharp.database.Models;
using advance_csharp.dto.Request.Product;
using advance_csharp.dto.Response.Product;
using Microsoft.EntityFrameworkCore;

namespace advance_csharp.service.Service
{
    public class ProductService : IProductService
    {

        private readonly DbContextOptions<AdvanceCsharpContext> dbContextOptions;

        public ProductService(DbContextOptions<AdvanceCsharpContext> dbContextOptions)
        {
            this.dbContextOptions = dbContextOptions;
        }

        /// <summary>
        /// ProductGetListResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ProductGetListResponse> GetApplicationProductList(ProductGetListRequest request)
        {
            ProductGetListResponse productGetListResponse = new()
            {
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };

            using (AdvanceCsharpContext context = new(dbContextOptions))
            {
                IQueryable<Product> query = context.Products ?? Enumerable.Empty<Product>().AsQueryable();
                if (query == null)
                {
                    return productGetListResponse;
                }

                if (!string.IsNullOrEmpty(request.Name))
                {
                    query = query.Where(a => a.Name.Contains(request.Name));
                }

                if (!string.IsNullOrEmpty(request.Category))
                {
                    query = query.Where(a => a.Category.Contains(request.Category));
                }

                if (!string.IsNullOrEmpty(request.PriceFrom))
                {
                    decimal priceFrom = Convert.ToDecimal(request.PriceFrom);
                    query = query.Where(a => Convert.ToDecimal(a.Price) >= priceFrom);
                }

                if (!string.IsNullOrEmpty(request.PriceTo))
                {
                    decimal priceTo = Convert.ToDecimal(request.PriceTo);
                    query = query.Where(a => Convert.ToDecimal(a.Price) <= priceTo);
                }

                // Count the total number of products according to filtered conditions
                productGetListResponse.TotalProduct = await query.CountAsync();

                // Calculate the number of pages and total pages
                int totalPages = (int)Math.Ceiling((double)productGetListResponse.TotalProduct / request.PageSize);
                productGetListResponse.TotalPages = totalPages;

                // Perform pagination and get data for the current page
                int startIndex = (request.PageIndex - 1) * request.PageSize;
                int endIndex = startIndex + request.PageSize;
                query = query.Skip(startIndex).Take(request.PageSize);

                productGetListResponse.Data = await query.Select(a => new ProductResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Category = a.Category,
                    Price = a.Price,
                    Quantity = a.Quantity,
                    Unit = a.Unit,
                    CreatedAt = a.CreatedAt,
                    IsDelete = a.IsDelete,
                }).ToListAsync();
            }

            return productGetListResponse;
        }

        /// <summary>
        /// create-product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ProductCreateResponse> CreateProduct(ProductCreateRequest request)
        {
            try
            {
                Product newProduct = new()
                {
                    Name = request.Name,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    Unit = request.Unit,
                    Images = request.Images,
                    Category = request.Category
                };

                using (AdvanceCsharpContext context = new(dbContextOptions))
                {
                    if (context.Products != null)
                    {
                        _ = context.Products.Add(newProduct);
                        _ = await context.SaveChangesAsync();
                    }
                }

                // create DTO to product info
                ProductResponse productResponse = new()
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Price = newProduct.Price,
                    Quantity = newProduct.Quantity,
                    Unit = newProduct.Unit,
                    Images = newProduct.Images,
                    Category = newProduct.Category,
                    IsDelete = newProduct.IsDelete,

                };

                // create DTO to respons
                ProductCreateResponse response = new()
                {
                    Message = "Product created successfully",
                    productResponse = productResponse
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ProductUpdateResponse> UpdateProduct(ProductUpdateRequest request)
        {
            try
            {
                // Check if the request is valid
                if (!request.IsPriceValid)
                {
                    return new ProductUpdateResponse
                    {
                        Message = "Invalid Price format. Please enter a valid number."
                    };
                }

                using AdvanceCsharpContext context = new(dbContextOptions);
                // Check if context.Products is null
                if (context.Products == null)
                {
                    // Handle the case where context.Products is null
                    return new ProductUpdateResponse
                    {
                        Message = "Error: context.Products is null."
                    };
                }

                // Check if the product exists
                Product? existingProduct = await context.Products.FindAsync(request.Id);

                if (existingProduct == null)
                {
                    return new ProductUpdateResponse
                    {
                        Message = "Product not found"
                    };
                }

                // Save old product information
                ProductResponse oldProduct = new()
                {
                    Id = existingProduct.Id,
                    Name = existingProduct.Name,
                    Price = existingProduct.Price,
                    Quantity = existingProduct.Quantity,
                    Unit = existingProduct.Unit,
                    Images = existingProduct.Images,
                    Category = existingProduct.Category,
                    CreatedAt = existingProduct.CreatedAt,
                    IsDelete = existingProduct.IsDelete,
                };

                // Update product information
                existingProduct.Name = request.Name;
                existingProduct.Price = request.Price;
                existingProduct.Quantity = request.Quantity;
                existingProduct.Unit = request.Unit;
                existingProduct.Images = request.Images;
                existingProduct.Category = request.Category;

                // Save changes to the database
                _ = await context.SaveChangesAsync();

                // Generate DTO for product information after update
                ProductResponse updatedProduct = new()
                {
                    Id = existingProduct.Id,
                    Name = existingProduct.Name,
                    Price = existingProduct.Price,
                    Quantity = existingProduct.Quantity,
                    Unit = existingProduct.Unit,
                    Images = existingProduct.Images,
                    Category = existingProduct.Category,
                    CreatedAt = existingProduct.CreatedAt

                };

                // Create DTO for response
                ProductUpdateResponse response = new()
                {
                    Message = "Product updated successfully",
                    OldProduct = oldProduct,
                    UpdatedProduct = updatedProduct
                };

                return response;
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine(ex.Message);
                return new ProductUpdateResponse
                {
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductDeleteResponse> DeleteProduct(Guid id)
        {
            try
            {
                using AdvanceCsharpContext context = new(dbContextOptions);
                // Check if context.Products is null
                if (context.Products == null)
                {
                    // Handle the case where context.Products is null
                    return new ProductDeleteResponse("Error: context.Products is null", new ProductResponse());
                }

                // Check if the product exists
                Product? existingProduct = await context.Products.FindAsync(id);

                if (existingProduct == null)
                {
                    return new ProductDeleteResponse("Product not found", new ProductResponse());
                }

                // Save old product information
                ProductResponse deletedProduct = new()
                {
                    Id = existingProduct.Id,
                    Name = existingProduct.Name ?? string.Empty,
                    Price = existingProduct.Price ?? string.Empty,
                    Quantity = existingProduct.Quantity,
                    Unit = existingProduct.Unit ?? string.Empty,
                    Images = existingProduct.Images ?? string.Empty,
                    Category = existingProduct.Category ?? string.Empty,
                    IsDelete = existingProduct.IsDelete,
                };

                // Remove the Product from the context (soft delete by setting IsDelete to true)
                deletedProduct.IsDelete = true;
                _ = context.Products.Update(existingProduct);

                // Save changes to the database
                _ = await context.SaveChangesAsync();

                // Returns a success message and information about the deleted product
                return new ProductDeleteResponse("Product deleted successfully", deletedProduct);
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine(ex.Message);
                return new ProductDeleteResponse("Error deleting product", new ProductResponse());
            }
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <summary>
        /// Get product by ID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Product> GetProductById(Guid productId)
        {
            try
            {
                using AdvanceCsharpContext context = new(dbContextOptions);
                if (context.Products == null)
                {
                    // Handle the case where context.Products is null
                    throw new InvalidOperationException("Context.Products is null");
                }

                // Retrieve the product by ID
                Product? product = await context.Products.FindAsync(productId);

                // Check if the product is null
                if (product != null)
                {
                    return product;
                }

                // Throw an exception indicating that the product is not found
                throw new KeyNotFoundException($"Product with ID {productId} not found");
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine($"Error while getting product by ID {productId}: {ex.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
        }

        /// <summary>
        /// GetProductPriceById
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<string> GetProductPriceById(Guid productId)
        {
            try
            {
                using AdvanceCsharpContext context = new(dbContextOptions);
                if (context.Products == null)
                {
                    // Handle the case where context.Products is null
                    throw new InvalidOperationException("Context.Products is null");
                }

                // Retrieve the product by ID
                Product? product = await context.Products.FindAsync(productId);

                // Check if the product is null
                if (product != null)
                {
                    // Return the product price
                    return product.Price;
                }

                // Throw an exception indicating that the product is not found
                throw new KeyNotFoundException($"Product with ID {productId} not found");
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine($"Error while getting product price by ID {productId}: {ex.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
        }
    }
}