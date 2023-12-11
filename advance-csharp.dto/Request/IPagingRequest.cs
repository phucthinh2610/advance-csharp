namespace advance_csharp.dto.Request
{
    /// <summary>
    /// Interface IPagingRequest
    /// </summary>
    public interface IPagingRequest
    {
        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Page Index
        /// </summary>
        public int PageIndex { get; set; }
    }
}