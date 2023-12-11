using advance_csharp.dto.Request.User;
using advance_csharp.dto.Response.User;

namespace advance_csharp.service.Interface
{
    public interface IUserService
    {
        /// <summary>
        /// Get application by Email and PhoneNumber string
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserGetListResponse> GetApplicationUserList(UserGetListRequest request);

        /// <summary>
        /// GetUserById
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserGetByIdResponse> GetUserById(UserGetByIdRequest request);

        /// <summary>
        /// CreateUser
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserCreateResponse> CreateUser(UserCreateRequest request);

        /// <summary>
        /// UpdateUser
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserUpdateResponse> UpdateUser(UserUpdateRequest request);

        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserDeleteResponse> DeleteUser(UserDeleteRequest request);

        /// <summary>
        /// SearchUser
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserSearchResponse> SearchUser(UserSearchRequest request);

        /*/// <summary>
        /// GenerateToken
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserGenerateTokenResponse> GenerateToken(UserGenerateTokenRequest request);*/
    }
}