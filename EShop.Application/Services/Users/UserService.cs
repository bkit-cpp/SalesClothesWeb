using EShop.Application.Interfaces;
using EShop.ViewModels.System.Users;
using EShop.ViewModels.Systems.Users;
using System;
using System.Text;
using System.Threading.Tasks;
using EShop.Data.EF;
using System.Linq;
using EShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EShop.ViewModels.Common;

namespace EShop.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly EShopDbContext _context;

        public UserService(EShopDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Login: Tạo một request đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<User>> Login(LoginRequest request)
        {
            try
            {
                byte[] str = Encoding.ASCII.GetBytes(request.Password);

               //query lấy tài khoản
               var query = from u in _context.Users
                            where u.UserName == request.UserName && u.Password == Convert.ToBase64String(str)
                            select new User();
                var result = query.FirstOrDefault();
                return new ApiSuccessResult<User>(result);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 400);
            }
        }

        /// <summary>
        /// Register: Đăng ký một user mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {

            try
            {
                if ( request.Password != request.ConfirmPassword)
                {
                    return new ApiErrorResult<bool>("Mật khẩu xác nhận không khớp");
                }
                byte[] str = Encoding.ASCII.GetBytes(request.Password);

                var user = new User()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Dob = request.Dob,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.UserName,
                    Password = Convert.ToBase64String(str)
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            } 
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("Đăng ký không thành công");
            }
        }

        /// <summary>
        /// GetUssersPaging: tìm kiếm phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetListUserPagingRequest request)
        {
            try
            {
                var query = from u in _context.Users
                            select new { u};

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.u.UserName.Contains(request.Keyword)
                     || x.u.PhoneNumber.Contains(request.Keyword));
                }

                //3. Paging
                int totalRow = await query.CountAsync();

                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new UserVm()
                    {
                        Email = x.u.Email,
                        PhoneNumber = x.u.PhoneNumber,
                        UserName = x.u.UserName,
                        FirstName = x.u.FirstName,
                        Id = x.u.Id,
                        LastName = x.u.LastName
                    }).ToListAsync();

                //4. Select and projection
                var pagedResult = new PagedResult<UserVm>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };
                return new ApiSuccessResult<PagedResult<UserVm>>(pagedResult);
            } catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 400);
            }

        }

        /// <summary>
        /// Delete: Xóa một user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return new ApiErrorResult<bool>("User không tồn tại");
                }
                 _context.Users.Remove(user);
                await  _context.SaveChangesAsync();

                return new ApiSuccessResult<bool>();
 
                
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 400);
            }

        }

        /// <summary>
        /// Update: Cập nhật thông tin user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return new ApiErrorResult<bool>($"Cannot find a user with id: {request.Id}");
                }

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Dob = request.Dob;
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;

                await _context.SaveChangesAsync();

                return new ApiSuccessResult<bool>();
            } catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }

        }

        public async Task<ApiResult<UserVm>> GetById(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return new ApiErrorResult<UserVm>("User không tồn tại");
                }
                var userVm = new UserVm()
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    Dob = user.Dob,
                    Id = user.Id,
                    LastName = user.LastName,
                    UserName = user.UserName
                };
                return new ApiSuccessResult<UserVm>(userVm);
            } catch (Exception ex)  
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }
    }
}
