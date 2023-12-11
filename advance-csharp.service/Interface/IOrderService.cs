using advance_csharp.dto.Request.Order;
using advance_csharp.dto.Response.Order;

namespace advance_csharp.service.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// CreateOrder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<OrderResponse> CreateOrder(OrderRequest request);

        /// <summary>
        /// Get rders by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<OrderListResponse> GetOrdersByUserId(OrderRequest orderRequest);

        /// <summary>
        /// GetAllOrders
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<GetAllOrderResponse> GetAllOrders(GetAllOrderRequest request);

        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        Task<UpdateOrderStatusResponse> UpdateOrderStatus(UpdateOrderStatusRequest request);

        /// <summary>
        /// DeleteOrder
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<OrderResponse> DeleteOrder(DeleteOrderRequest request);
    }
}