namespace advance_csharp.dto.Response.User
{
    public class UserDeleteResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Deleted User
        /// </summary>
        public UserResponse DeletedUser { get; set; }

        /// <summary>
        /// UserDeleteResponse
        /// </summary>
        /// <param name="message"></param>
        /// <param name="deletedUser"></param>
        public UserDeleteResponse(string message, UserResponse deletedUser)
        {
            Message = message;
            DeletedUser = deletedUser;
        }
    }
}