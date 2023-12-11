namespace advance_csharp.database.Models
{
    public class BaseResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}