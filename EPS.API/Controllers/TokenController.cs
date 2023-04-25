using EPS.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EPS.API.Helpers;
using EPS.Data.Entities;
using System.Globalization;
using EPS.API.Commons;
using EPS.Service.Dtos.User;
using EPS.Service;
using EPS.Service.Dtos.Password;
using EPS.Service.Dtos.Email;
using System.Collections.Generic;

namespace EPS.API.Controllers
{
    [Route("api/token")]
    public class TokenController : BaseController
    {
        private Data.EPSContext _context;
        private AuthorizationService _authorizationService;
        private EmailService _emailService;
        //some config in the appsettings.json
        private IOptions<Audience> _settings;
        //repository to handler the sqlite database

        public TokenController(IOptions<Audience> settings, Data.EPSContext context,AuthorizationService authorizationService, EmailService emailService)
        {
            this._settings = settings;
            _context = context;
            _authorizationService = authorizationService;
            _emailService = emailService;
        }
        
        [HttpPost("register")]
        public async Task<ApiResult<int>> CreateUser([FromForm] UserCreateDto newUser)
        {
            ApiResult<int> result = new ApiResult<int>();

            var isAdmin = newUser.RoleIds.Any(x => x == 1);
            if (isAdmin)
            {
                newUser.IsAdministrator = true;
            }
            newUser.backgroundImage = "avartar/user.png";
            var id = await _authorizationService.CreateUser(newUser);
            if (id == 0)
            {
                result.ResultObj = id;
                result.Message = "Tạo mới thành công !";
                result.statusCode = 201;
                return result;
            }
            else
            {
                result.ResultObj = id;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
            //return Ok(await _authorizationService.CreateUser(newUser));
        }

        [HttpPost("auth")]
        public async Task<ApiResult<string>> Auth(TokenRequestParams parameters)
        {
            ApiResult<string> result = new ApiResult<string>();
            if (!ModelState.IsValid)
            {
                result.ResultObj = "ModelState is valid .";
                result.Message = "ModelState is valid .";
                result.statusCode = 400;
                return result;
            }

            // Verify client's identification
            var client = await _context.IdentityClients.SingleOrDefaultAsync(x => x.IdentityClientId == parameters.client_id && x.SecretKey == parameters.client_secret);

            if (client == null)
            {
                result.ResultObj = "Unauthorized client.";
                result.Message = "Unauthorized client.";
                result.statusCode = 400;
                return result;
            }

            if (parameters.grant_type == "password")
            {
                return await DoPassword(parameters, client);
            }
            else if (parameters.grant_type == "refresh_token")
            {
                return await DoRefreshToken(parameters, client);
            }
            else if (parameters.grant_type == "invalidate_token")
            {
                return await DoInvalidateToken(parameters);
            }
            else
            {
                result.ResultObj = "Invalid grant type.";
                result.Message = "Invalid grant type.";
                result.statusCode = 400;
                return result;

            }
        }

        private async Task<ApiResult<string>> DoInvalidateToken(TokenRequestParams parameters)
        {
            ApiResult<string> result = new ApiResult<string>();
            var token = await _context.IdentityRefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == parameters.refresh_token);

            if (token == null)
            {
                result.ResultObj = "";
                result.Message = "";
                result.statusCode = 200;
                return result;
            }

            _context.Remove(token);

            await _context.SaveChangesAsync();
            result.ResultObj = "";
            result.Message = "";
            result.statusCode = 200;
            return result;
        }

        //scenario 1 ： get the access-token by username and password
        private async Task<ApiResult<string>> DoPassword(TokenRequestParams parameters, IdentityClient client)
        {
            ApiResult<string> result = new ApiResult<string>();
            //validate the client_id/client_secret/username/password                                          
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == parameters.username);

            if (user == null)
            {
                result.ResultObj = "Invalid user infomation.";
                result.Message = "Invalid user infomation.";
                result.statusCode = 400;
                return result;
            }

            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, parameters.password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                result.ResultObj = "Invalid user infomation.";
                result.Message = "Invalid user infomation.";
                result.statusCode = 400;
                return result;
            }
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

            var rToken = new IdentityRefreshToken
            {
                ClientId = parameters.client_id,
                RefreshToken = refresh_token,
                IdentityRefreshTokenId = Guid.NewGuid().ToString(),
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(client.RefreshTokenLifetime),
                Identity = parameters.username
            };

            //store the refresh_token
            _context.IdentityRefreshTokens.Add(rToken);

            await _context.SaveChangesAsync();
            result.Message = "";
            result.statusCode = 200;
            result.ResultObj = GetJwt(parameters.client_id, refresh_token, user);
            return result;
        }

        //scenario 2 ： get the access_token by refresh_token
        private async Task<ApiResult<string>> DoRefreshToken(TokenRequestParams parameters, IdentityClient client)
        {
            ApiResult<string> result = new ApiResult<string>();
            var token = await _context.IdentityRefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == parameters.refresh_token);

            if (token == null)
            {
                result.ResultObj = "Token not found.";
                result.Message = "Token not found.";
                result.statusCode = 400;
                return result;
            }

            if (token.IsExpired)
            {
                // Remove refresh token if expired
                _context.IdentityRefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
                result.ResultObj = "Token has expired.";
                result.Message = "Token has expired.";
                result.statusCode = 400;
                return result;
                
            }

            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");

            //remove the old refresh_token and add a new refresh_token
            _context.IdentityRefreshTokens.Remove(token);

            _context.IdentityRefreshTokens.Add(new IdentityRefreshToken
            {
                ClientId = parameters.client_id,
                RefreshToken = refresh_token,
                IdentityRefreshTokenId = Guid.NewGuid().ToString(),
                IssuedUtc = DateTime.Now,
                ExpiresUtc = DateTime.Now.AddDays(client.RefreshTokenLifetime),
                Identity = token.Identity
            });

            await _context.SaveChangesAsync();

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == token.Identity);

            if (user == null)
            {
                result.ResultObj = "User not found.";
                result.Message = "User not found.";
                result.statusCode = 400;
                return result;
            }
            result.ResultObj = GetJwt(parameters.client_id, refresh_token, user);
            result.Message = "";
            result.statusCode = 200;
            return result;
        }

        private string GetJwt(string client_id, string refresh_token, User user)
        {
            //var now = DateTime.UtcNow;

            var privileges = user.UserPrivileges.Select(x => x.PrivilegeId)
                .Union(user.UserRoles.SelectMany(x => x.Role.RolePrivileges.Select(y => y.PrivilegeId)))
                .Distinct()
                .ToList();

            var claims = new Claim[]
            {
                new Claim(CustomClaimTypes.ClientId, client_id),
                new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FullName, ClaimValueTypes.String),
                new Claim(ClaimTypes.NameIdentifier, user.UserName, ClaimValueTypes.String),
                new Claim(CustomClaimTypes.Privileges, string.Join(",", privileges)),
                new Claim(CustomClaimTypes.UnitId, user.UnitId.ToString())
            };

            var symmetricKeyAsBase64 = _settings.Value.Secret;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var expires = DateTime.Now.AddMinutes(60);
            var jwt = new JwtSecurityToken(
                issuer: _settings.Value.Iss,
                audience: _settings.Value.Aud,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                userId = user.Id,
                access_token = encodedJwt,
                expires,
                expiresString=expires.ToString("yyyy-MM-dd HH:MM:ss"),
                refresh_token,
                fullName = user.FullName,
                username = user.UserName,
                unitId = user.UnitId,
                claims = claims,
                privileges
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        [HttpPut("forgotpassword")]
        //quên mật khẩu
        public async Task<ApiResult<string>> forgotpassword(ForgotPasswordDto model)
        {
            ApiResult<string> result = new ApiResult<string>();
            var a = await _authorizationService.forgotPassword(UserIdentity.Username,model);
            if(a[0] == "1")
            {
                UserEmailOptions options = new UserEmailOptions()
                {
                    ToEmails = model.Email,
                    Subject = a[1]
            };

                List<string> infor = new List<string>();
                infor.Add("Success");
                await _emailService.SendMailConfirm(options, infor);

                result.ResultObj = a[1];
                result.Message = a[1];
                result.statusCode = 200;
                return result;
            }
            else
            {
                result.ResultObj = a[1];
                result.Message = a[1];
                result.statusCode = 404;
                return result;
            }
        }

        [HttpPut("password")]
        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordDto model)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            if (!ModelState.IsValid)
            {
                result.ResultObj = false;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 400;
                return result;
            }

            var check = await _authorizationService.ChangePassword(UserIdentity.Username, model);
            if (check)
            {
                result.ResultObj = check;
                result.Message = "Tạo mới thành công !";
                result.statusCode = 201;
                return result;
            }
            else
            {
                result.ResultObj = check;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
            //return Ok(await _authorizationService.ChangePassword(UserIdentity.Username, model));
        }
    }
}
