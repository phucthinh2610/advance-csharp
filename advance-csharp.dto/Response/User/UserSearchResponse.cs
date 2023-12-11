
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advance_csharp.dto.Response.User
{
    public class UserSearchResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// data User Response
        /// </summary>
        public List<UserResponse> Data { get; set; } = new List<UserResponse>();
    }
}