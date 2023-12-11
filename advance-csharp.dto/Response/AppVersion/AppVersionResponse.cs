namespace advance_csharp.dto.Response.AppVersion
{
    /// <summary>
    /// App Version Response
    /// </summary>
    public class AppVersionResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// IsDelete
        /// </summary>
        public bool IsDelete { get; set; }
    }
}