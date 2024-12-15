using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs;

public class UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
}