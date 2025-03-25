using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Auth;
using Server.Enums.Auth;
using Server.Enums.ErrorCodes;
using Server.Interfaces.IServices;
using Server.Middlewares;
using Server.Utils;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtUtils _jwtUtils;

        public AuthController(IAuthService authService, JwtUtils jwtUtils)
        {
            _authService = authService;
            _jwtUtils = jwtUtils;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginInfor)
        {
            try
            {
                var account = await _authService.Login(loginInfor.Email, loginInfor.Password);
                var user = await _authService.GetUser(account.Id);
                if (user == null)
                    return BadRequest(new { message = "User not found" });
                var token = _jwtUtils.GenerateToken(account.Id, account.Role, user.Id);
                return Ok(new { token });
            }
            catch (AuthException ex)
            {
                return ex.ErrorCode switch
                {
                    AuthErrorCode.AccountNotExist => BadRequest(new { message = "Account does not exist" }),
                    AuthErrorCode.InvalidPassword => BadRequest(new { message = "Invalid password" }),
                    AuthErrorCode.AccountNotActive => BadRequest(new { message = "Account is not active" }),
                    _ => BadRequest(new { message = "Unexpected error occurred" })
                };
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerInfor)
        {
            try
            {
                RoleCode role = (RoleCode)registerInfor.Role;
                await _authService.Register(registerInfor.Email, registerInfor.Password, role);
                return Ok(new { message = "Register successful" });
            }
            catch (AuthException ex)
            {
                return ex.ErrorCode switch
                {
                    AuthErrorCode.AccountAlreadyExist => BadRequest(new { message = "Account already exists" }),
                    _ => BadRequest(new { message = "Register failed" })
                };
            }
        }

        [HttpPost("request-active")]
        public async Task<IActionResult> RequestActive([FromBody] RequestActiveDTO requestActiveInfor)
        {
            try
            {
                var activeCode = await _authService.GetActiveCode(requestActiveInfor.Email, requestActiveInfor.Password);
                return Ok(new { activeCode });
            }
            catch (AuthException ex)
            {
                return ex.ErrorCode switch
                {
                    AuthErrorCode.AccountNotExist => BadRequest(new { message = "Account does not exist" }),
                    AuthErrorCode.InvalidPassword => BadRequest(new { message = "Invalid password" }),
                    AuthErrorCode.AccountAlreadyActivated => BadRequest(new { message = "Account already activated" }),
                    AuthErrorCode.RequestTooFrequent => BadRequest(new { message = "Request too frequent" }),
                    _ => BadRequest(new { message = "Unexpected error occurred" })
                };
            }
        }

        [HttpPost("activate-account")]
        public async Task<IActionResult> ActivateAccount([FromBody] ActiveAccountDTO activateAccountInfor)
        {
            try
            {
                await _authService.ActivateAccount(activateAccountInfor.ActiveCode);
                return Ok(new { message = "Account activated successfully" });
            }
            catch (AuthException ex)
            {
                return ex.ErrorCode switch
                {
                    AuthErrorCode.InvalidActivationCode => BadRequest(new { message = "Invalid code" }),
                    _ => BadRequest(new { message = "Unexpected error occurred" })
                };
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdateDTO changePasswordInfor)
        {
            try
            {
                var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(nameIdentifier, out var accountId))
                    return BadRequest(new { message = "Invalid user identifier" });

                await _authService.ChangePassword(accountId, changePasswordInfor.OldPassword, changePasswordInfor.NewPassword);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (AuthException ex)
            {
                return ex.ErrorCode switch
                {
                    AuthErrorCode.AccountNotExist => BadRequest(new { message = "Account not found" }),
                    AuthErrorCode.PasswordsNotMatch => BadRequest(new { message = "Old password is incorrect" }),
                    AuthErrorCode.OldPasswordMatching => BadRequest(new { message = "New password cannot be the same as the old one" }),
                    _ => BadRequest(new { message = "Unexpected error occurred" })
                };
            }
        }
    }
}
