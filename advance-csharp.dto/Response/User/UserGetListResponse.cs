namespace advance_csharp.dto.Response.User
{
    /// <summary>
    /// User GetList Response
    /// </summary>
    public class UserGetListResponse
    {
        /// <summary>
        /// Message
        /// </summary>
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

        /// <summary>
        /// Total Product
        /// </summary>
        public long TotalUser { get; set; }

        /// <summary>
        /// Data return
        /// </summary>
        public List<UserResponse> Data { get; set; } = new List<UserResponse>();
    }
}