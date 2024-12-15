using BankingApp.Application.DTOs;
using BankingApp.Application.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        
        return Ok(result);
    }

    [HttpGet]
    [Route("getAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _authService.GetUsers();
        
        return Ok(result);
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> GetUser(string userId)
    {
        var result = await _authService.FindByUserIdASync(userId);

        return Ok(result);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authService.RegisterUserAsync(registerDto);
        return result ? Ok("Registration successful") : BadRequest("Registration failed");
    }

    [HttpPut]
    [Route("update/{userId}")]
    public async Task<IActionResult> Update(string userId, [FromBody]UpdateDto updateDto)
    {
        var result = await _authService.UpdateUserAsync(userId, updateDto);
        return result ? Ok("User data updated successfully") : BadRequest("User data update failed");
    }

    [HttpDelete]
    [Route("delete")]
    [Authorize]
    public async Task<IActionResult> Delete(string userId)
    {
        var result = await _authService.DeleteUserAsync(userId);
        return result ? Ok("User deleted successfully") : BadRequest("User not found");
    }
}