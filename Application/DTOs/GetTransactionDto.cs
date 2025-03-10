namespace BankingApp.Application.DTOs;

public class GetTransactionDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Recipient { get; set; }
    public string Sender { get; set; }
    public DateTime Date { get; set; }
}