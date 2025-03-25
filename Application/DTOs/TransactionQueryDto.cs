namespace BankingApp.Application.DTOs;

public class TransactionQueryDto
{
    public string? UserId { get; set; }
    public string? CardId { get; set; }
    public string? IBAN { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? sortBy { get; set; }
    public bool SortAscending { get; set; }
    public int pageNumber { get; set; } = 1;
    public int pageSize { get; set; } = 10;
}