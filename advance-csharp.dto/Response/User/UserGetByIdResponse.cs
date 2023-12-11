namespace advance_csharp.dto.Response.User
{
    public class UserGetByIdResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Data User Response
        /// </summary>
        public UserResponse Data { get; set; } = new UserResponse();
    }
}