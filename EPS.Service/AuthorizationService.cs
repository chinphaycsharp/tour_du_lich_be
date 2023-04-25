using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Password;
using EPS.Service.Dtos.Role;
using EPS.Service.Dtos.User;
using EPS.Service.Helpers;
using EPS.Utils.Repository;
using EPS.Utils.Repository.Audit;
using EPS.Utils.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class AuthorizationService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private ILogger<AuthorizationService> _logger;
        private IUserIdentity<int> _userIdentity;

        public AuthorizationService(EPSRepository repository, IMapper mapper, UserManager<User> userManager, ILogger<AuthorizationService> logger, IUserIdentity<int> userIdentity)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _userIdentity = userIdentity;
            _baseService = new EPSBaseService(repository, mapper);
        }


        public async Task<List<string>> GetUserPrivileges(int userId)
        {
            return await _repository.Filter<UserPrivilege>(x => x.UserId == userId).Select(x => x.PrivilegeId).ToListAsync();
        }

        public async Task<int> GetUserByPrivilegeId(string PrivilegeId)
        {
            return await _repository.Filter<UserPrivilege>(x => x.PrivilegeId == PrivilegeId && x.UserId == _userIdentity.UserId).Select(x => x.UserId).SingleOrDefaultAsync();
        }

        public async Task<List<string>> GetRolePrivileges(int roleId)
        {
            return await _repository.Filter<RolePrivilege>(x => x.RoleId == roleId).Select(x => x.PrivilegeId).ToListAsync();
        }

        public async Task<int> GetRoleByPrivilegeId(string PrivilegeId)
        {
            return await _repository.Filter<RolePrivilege>(x => x.PrivilegeId == PrivilegeId).Select(x => x.RoleId).SingleOrDefaultAsync();
        }

        public async Task SaveUserPrivileges(int userId, string[] privileges)
        {
            using (var ts = _repository.BeginTransaction())
            {
                var db = _repository.GetDbContext<EPSContext>();
                var userPrivileges = await db.UserPrivileges.Include(x => x.Privilege).Where(x => x.UserId == userId).ToListAsync();

                foreach (var removingPrivilges in userPrivileges.Where(x => !privileges.Contains(x.PrivilegeId)))
                {
                    db.Remove(removingPrivilges);
                }

                foreach (var newPrivilege in privileges.Where(x => !userPrivileges.Any(y => y.PrivilegeId == x)))
                {
                    db.Add(new UserPrivilege() { UserId = userId, PrivilegeId = newPrivilege });
                }

                await db.SaveChangesAsync();

                ts.Commit();
            }
        }

        public async Task DeleteUserPrivileges(int userId, string[] privileges)
        {
            using (var ts = _repository.BeginTransaction())
            {
                var db = _repository.GetDbContext<EPSContext>();
                var userPrivileges = await db.UserPrivileges.Include(x => x.Privilege).Where(x => x.UserId == userId).ToListAsync();

                foreach (var removingPrivilges in userPrivileges.Where(x => privileges.Contains(x.PrivilegeId)))
                {
                    db.Remove(removingPrivilges);
                }

                await db.SaveChangesAsync();

                ts.Commit();
            }
        }

        public async Task SaveRolePrivileges(int roleId, string[] privileges)
        {
            using (var ts = _repository.BeginTransaction())
            {
                var db = _repository.GetDbContext<EPSContext>();
                var rolePrivileges = await db.RolePrivileges.Include(x => x.Privilege).Where(x => x.RoleId == roleId).ToListAsync();
                var a = rolePrivileges.Where(x => !privileges.Contains(x.PrivilegeId));
                foreach (var removingPrivilges in a)
                {
                    db.Remove(removingPrivilges);
                }

                var b = privileges.Where(x => !rolePrivileges.Any(y => y.PrivilegeId == x));
                foreach (var newPrivilege in b)
                {
                    db.Add(new RolePrivilege() { RoleId = roleId, PrivilegeId = newPrivilege });
                }

                await db.SaveChangesAsync();

                ts.Commit();
            }
        }

        public async Task DeleteRolePrivilege(int roleId, string[] privileges)
        {
            using (var ts = _repository.BeginTransaction())
            {
                var db = _repository.GetDbContext<EPSContext>();
                var rolePrivileges = await db.RolePrivileges.Include(x => x.Privilege).Where(x => x.RoleId == roleId).ToListAsync();

                foreach (var removingPrivilges in rolePrivileges.Where(x => privileges.Contains(x.PrivilegeId)))
                {
                    db.Remove(removingPrivilges);
                }

                await db.SaveChangesAsync();

                ts.Commit();
            }
        }

        public async Task SaveUserRoles(int userId, List<int> roles = null)
        {
            if (roles == null) roles = new List<int>();

            var db = _repository.GetDbContext<EPSContext>();
            var userRoles = await db.UserRoles.Include(x => x.Role).Where(x => x.UserId == userId).ToListAsync();

            foreach (var removingRole in userRoles.Where(x => !roles.Contains(x.RoleId)))
            {
                db.Remove(removingRole);
            }

            foreach (var newRole in roles.Where(x => !userRoles.Any(y => y.RoleId == x)))
            {
                db.Add(new UserRole() { UserId = userId, RoleId = newRole });
            }

            await db.SaveChangesAsync();
        }

        public async Task DeleteUserRoles(int userId, List<int> roles = null)
        {
            if (roles == null) roles = new List<int>();

            var db = _repository.GetDbContext<EPSContext>();
            var userRoles = await db.UserRoles.Include(x => x.Role).Where(x => x.UserId == userId).ToListAsync();
            var role = userRoles.Where(x => roles.Contains(x.RoleId)).ToList();

            foreach (var removingRole in userRoles.Where(x => roles.Contains(x.RoleId)))
            {
                db.Remove(removingRole);
            }

            await db.SaveChangesAsync();
        }

        public async Task<bool> ChangePassword(string userName, ChangePasswordDto model)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new Exception(string.Format(". \n\r", result.Errors.Select(x => x.Description)));
            }
        }

        public async Task<User> FindUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string[]> forgotPassword(string username,ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            string[] resultInt = new string[2];
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
              
                model.newPassword = randomPassString();
                user.PasswordVerificationToken = randomTokenString();
                user.DateValidateToken = DateTime.Now;
                await _userManager.UpdateAsync(user);
                //sendverificationemailpassword(entityuser, origin, model);
                var result = await _userManager.ResetPasswordAsync(user, token, model.newPassword);

                resultInt[0] = "1";
                resultInt[1] = model.newPassword;
                return resultInt;
            }
            else
            {
                resultInt[0] = "0";
                resultInt[1] = "Sai email hoặc email không tồn tại trong hệ thống !";
                return resultInt;
            }
        }

        private string randomTokenString()
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private string randomPassString()
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[3];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("_", "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:.[a-zA-Z0-9-]+)*$");
        }

        #region Role
        public async Task<int> CreateRole(RoleCreateDto roleCreate, bool isExploiting = false)
        {
            await _baseService.CreateAsync<Role, RoleCreateDto>(roleCreate);
            return roleCreate.Id;
        }

        public async Task<PagingResult<RoleGridDto>> GetRoles(RoleGridPagingDto pagingModel)
        {
            return await _baseService.FilterPagedAsync<Role, RoleGridDto>(pagingModel);
        }

        public async Task<int> DeleteRole(int id)
        {
            return await _baseService.DeleteAsync<Role, int>(id);
        }

        public async Task<int> DeleteRole(int[] ids)
        {
            return await _baseService.DeleteAsync<Role, int>(ids);
        }

        public async Task<RoleDetailDto> GetRoleById(int id)
        {
            return await _baseService.FindAsync<Role, RoleDetailDto>(id);
        }

        public async Task<int> UpdateRole(int id, RoleUpdateDto editedRole)
        {
            return await _baseService.UpdateAsync<Role, RoleUpdateDto>(id, editedRole);
        }
        #endregion

        #region User
        public async Task<int> CreateUser(UserCreateDto newUser)
        {
            using (var ts = _repository.BeginTransaction())
            {
                var entityUser = _mapper.Map<User>(newUser);

                var result = await _userManager.CreateAsync(entityUser, newUser.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(".", result.Errors.Select(x => x.Description));

                    throw new EPSException(errors);
                }
                else
                {
                    await SaveUserRoles(entityUser.Id, newUser.RoleIds);
                    // Must log manually if not using BaseService
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, default(EventId), new ExtraPropertyLogger("User {username} added new {entity} {identifier}", _userIdentity.Username, typeof(User).Name, entityUser.ToString()).AddProp("data", entityUser), null, ExtraPropertyLogger.Formatter);
                }

                ts.Commit();
                return newUser.Id;
            }
        }

        public async Task<PagingResult<UserGridDto>> GetUsers(UserGridPagingDto pagingModel)
        {
            //if (pagingModel.UnitId.GetValueOrDefault() <= 0)
            //{
            //    pagingModel.UnitId = _userIdentity.UnitId;
            //}
            return await _baseService.FilterPagedAsync<User, UserGridDto>(pagingModel);
        }

        public async Task<int> DeleteUser(int id)
        {
            var user = await _baseService.FindAsync<User, UserDetailDto>(id);

            await DeleteUserRoles(id, user.RoleIds);
            return await _baseService.DeleteAsync<User, int>(id);
        }

        public async Task<int> DeleteUser(int[] ids)
        {
            return await _baseService.DeleteAsync<User, int>(ids);
        }

        public async Task<UserDetailDto> GetUserById(int id)
        {
            return await _baseService.FindAsync<User, UserDetailDto>(id);
        }

        public async Task<bool> UpdateUser(int id, UserUpdateDto editedUser,List<int> roleIds)
        {
            using (var ts = _repository.BeginTransaction())
            {
                await _baseService.UpdateAsync<User, UserUpdateDto>(id, editedUser);
                //List<int> roleIds = editedUser.RoleIds.ToList();
                await SaveUserRoles(id, roleIds);
                if (!string.IsNullOrEmpty(editedUser.NewPassword))
                {
                    var user = await _baseService.GetDbContext<Data.EPSContext>().Users.FindAsync(id);
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var result = await _userManager.ResetPasswordAsync(user, token, editedUser.NewPassword);

                    if (!result.Succeeded)
                    {
                        throw new EPSException(string.Join(".", result.Errors.Select(x => x.Description)));
                    }
                    else
                    {
                        // Must log manually if not using BaseService
                        _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, default(EventId), string.Format("User {0} has changed password for user {1}", _userIdentity.Username, user.ToString()), null, ExtraPropertyLogger.Formatter);
                    }
                }

                ts.Commit();
                return true;
            }
        }
        #endregion

        public async Task<bool> UpdateImage(int id, ImageUpdateDto editedUser)
        {
            using (var ts = _repository.BeginTransaction())
            {
                await _baseService.UpdateAsync<User, ImageUpdateDto>(id, editedUser);
                ts.Commit();
                return true;
            }
        }

    }
}
