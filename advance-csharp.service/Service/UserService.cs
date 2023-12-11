using advance_csharp.service.Interface;
using advance_csharp.database;
using advance_csharp.database.Models;
using advance_csharp.dto.Request.User;
using advance_csharp.dto.Response.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace advance_Csharp.Service.Service
{
    public class UserService : IUserService
    {
        private readonly string email;
        private readonly DbContextOptions<AdvanceCsharpContext> dbContextOptions;

        public UserService(IHttpContextAccessor httpContextAccessor, DbContextOptions<AdvanceCsharpContext> dbContextOptions)
        {
            email = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
            this.dbContextOptions = dbContextOptions;
        }

        /// <summary>
        /// UserGetListResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserGetListResponse> GetApplicationUserList(UserGetListRequest request)
        {
            UserGetListResponse userGetListResponse = new()
            {
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };

            using (AdvanceCsharpContext context = new(dbContextOptions))
            {
                IQueryable<User> query = context.Users ?? Enumerable.Empty<User>().AsQueryable();
                if (query == null)
                {
                    return userGetListResponse;
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    query = query.Where(a => a.Email.Contains(request.Email));
                }

                if (!string.IsNullOrEmpty(request.PhoneNumber))
                {
                    query = query.Where(a => a.PhoneNumber.Contains(request.PhoneNumber));
                }

                // Count the total number of products according to filtered conditions
                userGetListResponse.TotalUser = await query.CountAsync();

                // Calculate the number of pages and total pages
                int totalPages = (int)Math.Ceiling((double)userGetListResponse.TotalUser / request.PageSize);
                userGetListResponse.TotalPages = totalPages;

                // Perform pagination and get data for the current page
                int startIndex = (request.PageIndex - 1) * request.PageSize;
                int endIndex = startIndex + request.PageSize;
                query = query.Skip(startIndex).Take(request.PageSize);

                userGetListResponse.Data = await query.Select(a => new UserResponse
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Email = a.Email,
                    Password = a.Password,
                    PhoneNumber = a.PhoneNumber,
                    Address = a.Address,
                    CreatedAt = a.CreatedAt,
                    IsDelete = a.IsDelete,
                }).ToListAsync();
            }

            return userGetListResponse;
        }

        /// <summary>
        /// GetUserById
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserGetByIdResponse> GetUserById(UserGetByIdRequest request)
        {
            UserGetByIdResponse userGetByIdResponse = new();

            try
            {
                using AdvanceCsharpContext context = new(dbContextOptions);
                // Check if context.Users is null
                if (context.Users == null)
                {
                    // Handle the case where context.Users is null
                    return new UserGetByIdResponse
                    {
                        Message = "Error: context.Users is null."
                    };
                }

                // Query the user from the database based on Id
                User? user = await context.Users.FindAsync(request.Id);

                // Check if the user is not found
                if (user == null)
                {
                    // Handle the case where the user is not found
                    userGetByIdResponse.Message = "User not found";
                }
                else
                {
                    // Convert user information to UserResponse object
                    userGetByIdResponse.Data = new UserResponse
                    {
                        Id = user.Id,
                        LastName = user.LastName,
                        FirstName = user.FirstName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        CreatedAt = user.CreatedAt,
                        IsDelete = user.IsDelete,
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                userGetByIdResponse.Message = $"An error occurred while retrieving user information: {ex.Message}";
            }

            return userGetByIdResponse;
        }

        /// <summary>
        /// create-User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserCreateResponse> CreateUser(UserCreateRequest request)
        {
            try
            {
                User newUser = new()
                {
                    LastName = request.LastName,
                    FirstName = request.FirstName,
                    Email = request.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,

                };

                using (AdvanceCsharpContext context = new(dbContextOptions))
                {
                    // Check if the Users table is not null
                    if (context.Users != null)
                    {
                        _ = context.Users.Add(newUser);
                        _ = await context.SaveChangesAsync();

                        // Create a new cart for the user (without cart details initially)
                        Cart newCart = new()
                        {
                            UserId = newUser.Id,
                            CartDetails = new List<CartDetail>()
                        };

                        // Add the new cart to the context
                        _ = context.Carts.Add(newCart);
                        _ = await context.SaveChangesAsync();
                    }
                }

                // create DTO to user info
                UserResponse userResponse = new()
                {
                    Id = newUser.Id,
                    LastName = newUser.LastName,
                    FirstName = newUser.FirstName,
                    Email = newUser.Email,
                    Password = newUser.Password,
                    PhoneNumber = newUser.PhoneNumber,
                    Address = newUser.Address,
                    IsDelete = false,
                };

                // create DTO to respons
                UserCreateResponse response = new()
                {
                    Message = "User created successfully",
                    UserResponse = userResponse
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserUpdateResponse> UpdateUser(UserUpdateRequest request)
        {
            try
            {
                // Validate phone number
                if (!request.IsPhoneValid)
                {
                    return new UserUpdateResponse
                    {
                        Message = "Invalid phone number format. Please enter a valid number."
                    };
                }

                using AdvanceCsharpContext context = new(dbContextOptions);
                if (context.Users == null)
                {
                    return new UserUpdateResponse
                    {
                        Message = "Error: context.Users is null."
                    };
                }

                User? existingUser = await context.Users.FindAsync(request.Id);

                if (existingUser == null)
                {
                    return new UserUpdateResponse
                    {
                        Message = "User not found."
                    };
                }

                // Save old user information
                UserResponse oldUser = new()
                {
                    Id = existingUser.Id,
                    LastName = existingUser.LastName,
                    FirstName = existingUser.FirstName,
                    Email = existingUser.Email,
                    Password = existingUser.Password,
                    PhoneNumber = existingUser.PhoneNumber,
                    Address = existingUser.Address,
                    CreatedAt = existingUser.CreatedAt,
                    IsDelete = existingUser.IsDelete,
                };

                // Update user information
                existingUser.LastName = request.LastName;
                existingUser.FirstName = request.FirstName;
                existingUser.Email = request.Email;

                if (!string.IsNullOrEmpty(request.Password))
                {
                    existingUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                }

                existingUser.PhoneNumber = request.PhoneNumber;
                existingUser.Address = request.Address;

                // Save changes to the database
                _ = await context.SaveChangesAsync();

                // Generate DTO for user information after update
                UserResponse updatedUser = new()
                {
                    Id = existingUser.Id,
                    LastName = existingUser.LastName,
                    FirstName = existingUser.FirstName,
                    Email = existingUser.Email,
                    Password = existingUser.Password,
                    PhoneNumber = existingUser.PhoneNumber,
                    Address = existingUser.Address,
                    CreatedAt = existingUser.CreatedAt,
                    IsDelete = existingUser.IsDelete,
                };

                // Create DTO for response
                UserUpdateResponse response = new()
                {
                    Message = "User updated successfully",
                    OldUser = oldUser,
                    UpdatedUser = updatedUser
                };

                return response;
            }
            catch (DbUpdateException dbEx)
            {
                // Log specific database update exceptions
                Console.WriteLine(dbEx.Message);
                return new UserUpdateResponse
                {
                    Message = "Error updating user. Please try again later."
                };
            }
            catch (Exception ex)
            {
                // Log detailed error information
                Console.WriteLine(ex.ToString());

                return new UserUpdateResponse
                {
                    Message = "An unexpected error occurred. Please contact support."
                };
            }
        }

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDeleteResponse> DeleteUser(UserDeleteRequest request)
        {
            try
            {
                using AdvanceCsharpContext context = new(dbContextOptions);

                // Check if context.Users is null
                if (context.Users == null)
                {
                    // Handle the case where context.Users is null
                    return new UserDeleteResponse("Error: context.Users is null", new UserResponse());
                }

                // Check if the user exists
                User? existingUser = await context.Users.FindAsync(request.Id);

                if (existingUser == null)
                {
                    return new UserDeleteResponse("User not found", new UserResponse());
                }

                // Save old user information
                UserResponse deletedUser = new()
                {
                    Id = existingUser.Id,
                    LastName = existingUser.LastName ?? string.Empty,
                    FirstName = existingUser.FirstName ?? string.Empty,
                    Email = existingUser.Email,
                    Password = existingUser.Password ?? string.Empty,
                    PhoneNumber = existingUser.PhoneNumber ?? string.Empty,
                    Address = existingUser.Address ?? string.Empty,
                    IsDelete = true,
                };

                // Soft delete by setting IsDelete to true
                existingUser.IsDelete = true;
                _ = context.Users.Update(existingUser);

                // Save changes to the database
                _ = await context.SaveChangesAsync();

                // Returns a success message and information about the deleted user
                return new UserDeleteResponse("User deleted successfully", deletedUser);
            }
            catch (Exception ex)
            {
                // Log errors or send errors to a logging service
                Console.WriteLine(ex.Message);
                return new UserDeleteResponse("Error deleting user", new UserResponse());
            }
        }

        /// <summary>
        /// SearchUser
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserSearchResponse> SearchUser(UserSearchRequest request)
        {
            UserSearchResponse response = new();

            try
            {
                using AdvanceCsharpContext context = new(dbContextOptions);
                IQueryable<User> query = context.Users ?? Enumerable.Empty<User>().AsQueryable();

                if (query == null)
                {
                    response.Message = "Error: Query is null.";
                    return response;
                }

                if (!string.IsNullOrEmpty(email))
                {
                    query = query.Where(a => a.Email.Contains(email));
                }

                List<UserResponse> matchingUsers = await query.Select(a => new UserResponse
                {
                    Id = a.Id,
                    LastName = a.LastName,
                    FirstName = a.FirstName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Address = a.Address,
                    CreatedAt = a.CreatedAt,
                    IsDelete = a.IsDelete,
                }).ToListAsync();

                if (matchingUsers.Any())
                {
                    response.Data.AddRange(matchingUsers);
                }
                else
                {
                    response.Message = "No matching entities found.";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or rethrow
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        
    }
}