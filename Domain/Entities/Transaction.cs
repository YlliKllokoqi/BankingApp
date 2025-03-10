namespace BankingApp.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid SourceDebitCardId { get; set; }
    public string Recipient { get; set; }
    public string Sender { get; set; }
    public string IBAN { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}