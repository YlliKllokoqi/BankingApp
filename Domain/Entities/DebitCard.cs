namespace BankingApp.Domain.Entities;

public class DebitCard
{
    public Guid Id { get; set; }
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Balance { get; set; }
    public string Status { get; set; }
    public string IBAN { get; set; }
    public string OwnerName { get; set; }
    public string OwnerId { get; set; }
    public ApplicationUser Owner { get; set; }
}