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
        var result = await _transactionService.TransferFunds(transactionDto);

        if (result.IsSuccess)
            return Ok("Transaction completed successfully");
        
        return BadRequest(result);
    }
}