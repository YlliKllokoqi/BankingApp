namespace BankingApp.Application.DTOs;

public class TransactionDto
{
    public Guid SourceDebitCardId { get; set; }
    public Guid DestinationDebitCardId { get; set; }
    public decimal Amount { get; set; }
}