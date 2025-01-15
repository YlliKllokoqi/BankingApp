namespace BankingApp.Application.DTOs;

public class DebitCardDto
{
    public Guid Id { get; set; }
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Balance { get; set; }
    public string IBAN { get; set; }
    public string OwnerName { get; set; }
}