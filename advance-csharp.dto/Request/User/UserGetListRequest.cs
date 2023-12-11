namespace advance_csharp.dto.Request.User
{
    public class UserGetListRequest : IPagingRequest
    {
        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Page Index
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// Search: Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone Number
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }
}