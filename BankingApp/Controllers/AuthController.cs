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

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error.Message, details = result.Error.Details });

        return Ok(result.Data);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _authService.GetUsers();

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error.Message });

        return Ok(result.Data);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authService.RegisterUserAsync(registerDto);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error.Message, details = result.Error.Details });

        return Ok(new { message = "Registration successful" });
    }

    [HttpPut("update/{userId}")]
    public async Task<IActionResult> Update(string userId, UpdateDto updateDto)
    {
        var result = await _authService.UpdateUserAsync(userId, updateDto);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error.Message, details = result.Error.Details });

        return Ok(new { message = "User updated successfully" });
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> Delete(string userId)
    {
        var result = await _authService.DeleteUserAsync(userId);

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error.Message, details = result.Error.Details });

        return Ok(new { message = "User deleted successfully" });
    }


}