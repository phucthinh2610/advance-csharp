using advance_csharp.service.Interface;
using advance_csharp.database;
using advance_csharp.database.Models;
using advance_csharp.dto.Request.Order;
using advance_csharp.dto.Response.Order;
using advance_csharp.service.Interface;
using Microsoft.EntityFrameworkCore;

namespace advance_csharp.service.Service
{
    public class OrderService : IOrderService
    {
        private readonly DbContextOptions<AdvanceCsharpContext> _context;
        public OrderService(DbContextOptions<AdvanceCsharpContext> dbContextOptions)
        {
            _context = dbContextOptions;
        }

        /// <summary>
        /// CreateOrder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OrderResponse> CreateOrder(OrderRequest request)
        {
            try
            {
                using AdvanceCsharpContext context = new(_context);

                // Fetch cart details based on the provided userId, excluding deleted items
                List<CartDetail> cartDetails = await FetchCartDetails(request.UserId);

                // Check if all cartDetails have IsDelete set to true
                if (cartDetails.All(cd => cd.IsDelete))
                {
                    // Return an error response indicating there are no products in the cart
                    return new OrderResponse
                    {
                        Message = "There aren't products in the cart"
                    };
                }

                // Calculate the total amount from fetched cart details
                decimal totalAmount = CalculateTotalAmount(cartDetails);

                // Create an order
                Order order = new()
                {
                    UserId = request.UserId,
                    OrderDate = DateTimeOffset.UtcNow,
                    TotalAmount = totalAmount,
                    OrderDetails = cartDetails?.Select(cartDetail => new OrderDetail
                    {
                        ProductId = cartDetail.ProductId,
                        Price = decimal.TryParse(cartDetail.Price, out decimal price) ? price : 0,
                        Quantity = cartDetail.Quantity,
                        OrderStatus = false
                    }).ToList() ?? new List<OrderDetail>()
                };

                // Save the order to the database
                _ = context.Orders?.Add(order);
                _ = await context.SaveChangesAsync();

                // Remove cart items after creating the order
                if (cartDetails != null && cartDetails.Any())
                {
                    foreach (CartDetail cartDetail in cartDetails)
                    {
                        // Check if cartDetail is not null
                        if (cartDetail != null)
                        {
                            // Set isDelete to true for the cart detail
                            cartDetail.IsDelete = true;

                            // Update the cart detail in the context
                            _ = (context.CartDetails?.Update(cartDetail));
                        }
                        else
                        {
                            // Handle the case where cartDetail is null
                            Console.WriteLine("Error: cartDetail is null");
                        }
                    }

                    _ = await context.SaveChangesAsync();
                }

                // Create OrderResponse
                OrderResponse orderResponse = new()
                {
                    Message = "Create order success for User Id: " + order.UserId,
                    OrderId = order.Id,
                    UserId = order.UserId,
                    TotalAmount = order.TotalAmount,
                    OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponse
                    {
                        Id = od.Id.GetValueOrDefault(),
                        ProductId = od.ProductId,
                        Price = od.Price,
                        Quantity = od.Quantity,
                        OrderStatus = od.OrderStatus // Include OrderStatus in the response
                    }).ToList() ?? new List<OrderDetailResponse>(),
                    // Add other information if needed
                };

                return orderResponse;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                Console.WriteLine($"An error occurred while creating the order: {ex.Message}");
                return new OrderResponse
                {
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// GetAllOrders
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetAllOrderResponse> GetAllOrders(GetAllOrderRequest request)
        {
            try
            {
                using AdvanceCsharpContext context = new(_context);
                // Create the initial query
                IQueryable<Order> query = context.Orders ?? Enumerable.Empty<Order>().AsQueryable();

                // Count the total number of orders according to filtered conditions
                long totalOrders = await query.CountAsync();

                // Calculate the number of pages and total pages
                int totalPages = (int)Math.Ceiling((double)totalOrders / request.PageSize);

                // Perform pagination and get data for the current page
                int startIndex = (request.PageIndex - 1) * request.PageSize;
                int endIndex = startIndex + request.PageSize;
                query = query.Skip(startIndex).Take(request.PageSize);

                // Fetch orders from the database
                List<Order> orders = await query
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                // Create the response
                GetAllOrderResponse response = new()
                {
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    TotalPages = totalPages,
                    TotalOrder = totalOrders,
                    Orders = orders.Select(o => new OrderResponse
                    {
                        OrderId = o.Id,
                        UserId = o.UserId,
                        OrderDate = o.OrderDate.HasValue ? o.OrderDate.Value
                                        .ToOffset(new TimeSpan(7, 0, 0)) : DateTimeOffset.MinValue,
                        TotalAmount = o.TotalAmount,
                        OrderDetails = o.OrderDetails?.Select(od => new OrderDetailResponse
                        {
                            Id = od.Id.GetValueOrDefault(),
                            ProductId = od.ProductId,
                            Price = od.Price,
                            Quantity = od.Quantity,
                            OrderStatus = od.OrderStatus
                        }).ToList() ?? new List<OrderDetailResponse>()
                    }).ToList()
                };

                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                Console.WriteLine($"An error occurred while retrieving all orders: {ex.Message}");
                return new GetAllOrderResponse
                {
                    TotalOrder = 0, // Set TotalOrder to 0 in case of an error
                    Orders = new List<OrderResponse>() // Set Orders to an empty list
                };
            }
        }

        /// <summary>
        /// Get orders by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OrderListResponse> GetOrdersByUserId(OrderRequest orderRequest)
        {
            try
            {
                if (orderRequest == null)
                {
                    // Handle the case where the request is null
                    Console.WriteLine("Error: OrderRequest is null");
                    return new OrderListResponse { Orders = new List<OrderResponse>() };
                }

                using AdvanceCsharpContext context = new(_context);

                // Check if context.Orders is not null
                if (context.Orders != null)
                {
                    // Retrieve the orders for the given user ID
                    List<OrderResponse> orderResponses = await context.Orders
                        .Include(o => o.OrderDetails)
                        .Where(o => o.UserId == orderRequest.UserId)
                        .Select(o => new OrderResponse
                        {
                            Message = "Order information of UserId" + orderRequest.UserId,
                            OrderId = o.Id,
                            UserId = o.UserId,
                            OrderDate = o.OrderDate.HasValue ? o.OrderDate.Value
                                            .ToOffset(new TimeSpan(7, 0, 0)) : DateTimeOffset.MinValue,
                            TotalAmount = o.TotalAmount,
                            OrderDetails = (o.OrderDetails != null) ? o.OrderDetails.Select(od => new OrderDetailResponse
                            {
                                Id = od.Id.GetValueOrDefault(),
                                ProductId = od.ProductId,
                                Price = od.Price,
                                Quantity = od.Quantity,
                                OrderStatus = od.OrderStatus,
                                OrderStatusDescription = od.OrderStatus ? "Đã thanh toán" : "Chưa thanh toán"
                                // Add other properties as needed
                            }).ToList() : new List<OrderDetailResponse>()
                        })
                        .ToListAsync();

                    // Create the OrderListResponse
                    OrderListResponse orderListResponse = new()
                    {
                        Orders = orderResponses
                    };

                    return orderListResponse;
                }
                else
                {
                    // Handle the case where context.Orders is null
                    Console.WriteLine("Error: context.Orders is null");

                    // Return an empty OrderListResponse or take other appropriate actions
                    return new OrderListResponse { Orders = new List<OrderResponse>() };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                Console.WriteLine($"An error occurred while getting orders: {ex.Message}");

                // Return an empty OrderListResponse or take other appropriate actions
                return new OrderListResponse { Orders = new List<OrderResponse>() };
            }
        }

        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        public async Task<UpdateOrderStatusResponse> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            UpdateOrderStatusResponse response = new();

            try
            {
                using AdvanceCsharpContext context = new(_context);
                // Retrieve the order to be updated based on userId
                Order? order = null;

                if (context.Orders != null)
                {
                    order = await context.Orders
                        .Where(o => o.UserId == request.UserId)
                        .Include(o => o.OrderDetails)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    // Handle the case where context.Orders is null
                    Console.WriteLine("Error: context.Orders is null");
                }

                if (order != null)
                {
                    // Update the order status
                    order.OrderDetails?.ForEach(od => od.OrderStatus = request.NewStatus);

                    // Save changes to the database
                    _ = await context.SaveChangesAsync();

                    // If the order status is updated to true, reduce product quantities
                    if (request.NewStatus)
                    {
                        using AdvanceCsharpContext productContext = new(_context);
                        if (order.OrderDetails != null && productContext.Products != null)
                        {
                            // ... (existing code)
                        }
                    }

                    // Set the success response and message
                    response.Success = true;
                    response.Message = "Order status updated successfully.";

                    // Create OrderResponse directly
                    response.UpdatedOrder = new OrderResponse
                    {
                        Message = $"Success for User Id: {order.UserId}",
                        OrderId = order.Id,
                        UserId = order.UserId,
                        TotalAmount = order.TotalAmount,
                        OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponse
                        {
                            Id = od.Id.GetValueOrDefault(),
                            ProductId = od.ProductId,
                            Price = od.Price,
                            Quantity = od.Quantity,
                            OrderStatus = od.OrderStatus
                        }).ToList() ?? new List<OrderDetailResponse>(),
                        // Add other information if needed
                    };
                }
                else
                {
                    // If the order is not found
                    response.Message = $"Order with UserId {request.UserId} not found for updating status.";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                Console.WriteLine($"An error occurred while updating the order status: {ex.Message}");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// DeleteOrder by Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderResponse> DeleteOrder(DeleteOrderRequest request)
        {
            try
            {
                if (request == null)
                {
                    // Handle the case where the request is null
                    Console.WriteLine("Error: DeleteOrderRequest is null");
                    return new OrderResponse();
                }

                using AdvanceCsharpContext context = new(_context);

                // Check if context.Orders is not null before querying
                if (context.Orders != null)
                {
                    // Retrieve the order to be deleted
                    Order? orderToDelete = await context.Orders
                        .Where(o => o.Id == request.OrderId)
                        .Include(o => o.OrderDetails)
                        .FirstOrDefaultAsync();

                    // Check if the order exists
                    if (orderToDelete != null)
                    {
                        // Create an OrderResponse object with the order information before deletion
                        OrderResponse deletedOrderResponse = new()
                        {
                            OrderId = orderToDelete.Id,
                            UserId = orderToDelete.UserId,
                            TotalAmount = orderToDelete.TotalAmount,
                            OrderDetails = orderToDelete.OrderDetails?.Select(od => new OrderDetailResponse
                            {
                                Id = od.Id.GetValueOrDefault(),
                                ProductId = od.ProductId,
                                Price = od.Price,
                                Quantity = od.Quantity,
                                OrderStatus = od.OrderStatus,
                                OrderStatusDescription = od.OrderStatus ? "Đã thanh toán" : "Chưa thanh toán"
                            }).ToList() ?? new List<OrderDetailResponse>()
                        };

                        orderToDelete.IsDelete = true;
                        _ = context.Orders.Update(orderToDelete);

                        // Save changes to the database
                        _ = await context.SaveChangesAsync();

                        // Display a message indicating successful deletion
                        Console.WriteLine($"Order with OrderId {request.OrderId} has been successfully deleted.");

                        // Return the deleted order information
                        return deletedOrderResponse;
                    }
                    else
                    {
                        // Display a message indicating that the order was not found
                        Console.WriteLine($"Order with OrderId {request.OrderId} not found for deletion.");
                        return new OrderResponse();
                    }
                }
                else
                {
                    // Handle the case where context.Orders is null
                    Console.WriteLine("Error: context.Orders is null");
                    return new OrderResponse();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                Console.WriteLine($"An error occurred while deleting the order: {ex.Message}");
                return new OrderResponse();
            }
        }

        /// <summary>
        /// CalculateTotalAmount
        /// </summary>
        /// <param name="cartDetails"></param>
        /// <returns></returns>
        private static decimal CalculateTotalAmount(List<CartDetail>? cartDetails)
        {
            // Check for null before calculating the total amount
            if (cartDetails == null || !cartDetails.Any())
            {
                return 0;
            }

            // Calculate the total amount from cart details
            return cartDetails.Sum(cartDetail => decimal.TryParse(cartDetail.Price, out decimal price) ? price * cartDetail.Quantity : 0);
        }

        /// <summary>
        /// FetchCartDetails
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<List<CartDetail>> FetchCartDetails(Guid userId)
        {
            using AdvanceCsharpContext context = new(_context);
            DbSet<CartDetail>? cartDetailsQuery = context.CartDetails;

            if (cartDetailsQuery != null)
            {
                return await cartDetailsQuery
                    .Where(cd => cd.Cart != null && cd.Cart.UserId == userId && !cd.IsDelete)
                    .ToListAsync();
            }
            else
            {
                // Handle the case where context.CartDetails is null
                Console.WriteLine("Error: context.CartDetails is null");
                return new List<CartDetail>();
            }
        }
    }
}