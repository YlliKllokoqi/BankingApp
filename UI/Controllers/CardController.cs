using System.Security.Claims;
using BankingApp.Application.Services.DebitCard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardController : ControllerBase
{
    private readonly IDebitCardService _debitCardService;

    public CardController(IDebitCardService debitCardService)
    {
        _debitCardService = debitCardService;
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpPost("RequestDebitCard")]
    public async Task<IActionResult> RequestDebitCard()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Invalid User");
        
        var result = await _debitCardService.RequestDebitCard(userId);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok("Debit card Application was successful, your debit card is now pending approval.");
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("approveDebitCard/{debitCardId}")]
    public async Task<IActionResult> ApproveDebitCard(Guid debitCardId)
    {
        var result = await _debitCardService.ApproveDebitCard(debitCardId);

        if (result.IsSuccess)
        {
            return Ok("Debit card successfully approved.");
        }
        
        return BadRequest(result);
    }

    [Authorize]
    [HttpGet("GetDebitCardDetails")]
    public async Task<IActionResult> GetDebitCardDetails([FromQuery] string? userId = null)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = User.IsInRole("Admin");

        if (!string.IsNullOrEmpty(userId) && !isAdmin) 
            return Forbid();
        
        var targetUserId = isAdmin && !string.IsNullOrEmpty(userId) ? userId : currentUserId;

        var result = await _debitCardService.GetDebitCardDetails(targetUserId);
        
        if(result.IsSuccess)
            return Ok(result.Data);
        
        return BadRequest(result);
    }

    [Authorize]
    [HttpPut("DepositToBalance/{debitCardId}")]
    public async Task<IActionResult> DepositToBalance(Guid debitCardId, decimal amount)
    {
        var result = await _debitCardService.DepositToBalance(debitCardId, amount);

        if (result.IsSuccess)
            return Ok(result.Data);

        return BadRequest(result);
    }

    [Authorize]
    [HttpPut("WithdrawFromBalance/{debitCardId}")]
    public async Task<IActionResult> WithdrawFromBalance(Guid debitCardId, decimal amount)
    {
        var result = await _debitCardService.WithdrawFromBalance(debitCardId, amount);
        
        if(result.IsSuccess)
            return Ok(result.Data);
        
        return BadRequest(result);
    }
}