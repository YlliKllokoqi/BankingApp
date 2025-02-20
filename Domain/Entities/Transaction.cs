namespace BankingApp.Domain.Entities;

public class Transaction
{
    public Guid SourceDebitCardId { get; set; }
    public Guid DestinationDebitCardId { get; set; }
    public decimal Amount { get; set; }
}