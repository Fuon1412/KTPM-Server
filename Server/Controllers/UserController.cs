using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.User;
using Server.Enums.ErrorCodes;
using Server.Interfaces.IServices;
using Server.Middlewares;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try {
                var name = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(name, out var accountId))
                    return BadRequest(new { message = "Invalid user identifier" });
                var profile = await _userService.GetProfileAsync(accountId);
                return Ok(profile);
            } catch(UserException ex) {
                return ex.ErrorCode switch
                {
                    UserErrorCode.UserNotFound => BadRequest(new { message = "User does not exist" }),
                    _ => BadRequest(new { message = "Unexpected error occurred" })
                };
            }
        }

        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            try {
                var name = User.FindFirst(ClaimTypes.Name)?.Value;
                if (!Guid.TryParse(name, out var userId))
                    return BadRequest(new { message = "Invalid user identifier" });
                await _userService.UpdateProfileAsync(userId, model);
                return Ok(new { message = "Update profile successful" });
            } catch(UserException ex) {
                return ex.ErrorCode switch
                {
                    UserErrorCode.UserNotFound => BadRequest(new { message = "User does not exist" }),
                    _ => BadRequest(new { message = "Unexpected error occurred" })
                };
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("all-user")]
        public async Task<IActionResult> GetAllUser()
        {
            try {
                var users = await _userService.GetUsersAsync();
                return Ok(users);
            } catch(UserException ex) {
                return ex.ErrorCode switch
                {
                    UserErrorCode.UnknownError => BadRequest(new { message = "Unknown error" }),
                    _ => BadRequest(new { message = "Unexpected error occurred" })
                };
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-account")]
        public async Task<IActionResult> GetAllAccount()
        {
            try {
                var accounts = await _userService.GetAccountsAsync();
                return Ok(accounts);
            } catch(UserException ex) {
                return ex.ErrorCode switch
                {
                    UserErrorCode.UnknownError => BadRequest(new { message = "Unknown error" }),
                    _ => BadRequest(new { message = "Unexpected error occurred" })
                };
            }
        }
    }
}