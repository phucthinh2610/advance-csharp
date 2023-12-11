namespace advance_csharp.dto.Request.Order
{
    public class UpdateOrderStatusRequest
    {
        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// NewStatus
        /// </summary>
        public bool NewStatus { get; set; } = false;
    }
}