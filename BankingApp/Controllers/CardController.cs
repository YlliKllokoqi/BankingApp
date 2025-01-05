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

    [HttpPost("RequestDebitCard")]
    [Authorize(Policy = "UserPolicy")]
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
}