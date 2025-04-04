using System.Security.Claims;
using BankingApp.Application.DTOs;
using BankingApp.Application.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [Authorize]
    [HttpPost("TransferFunds")]
    public async Task<IActionResult> TransferFunds([FromBody] TransactionDto transactionDto)
    {
        var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _transactionService.TransferFunds(transactionDto, currentUser);

        if (result.IsSuccess)
            return Ok("Transaction completed successfully");
        
        return BadRequest(result);
    }

    [HttpGet("GetTransactionHistory")]
    public async Task<IActionResult> GetTransactionHistory([FromQuery] TransactionQueryDto queryDto)
    {
        if (queryDto.StartDate > queryDto.EndDate)
        {
            return BadRequest("Start date must be before end date.");
        }

        if (string.IsNullOrEmpty(queryDto.UserId))
        {
            queryDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        var transactions = await _transactionService.GetTransactionHistory(queryDto);

        return Ok(transactions);
    }
}