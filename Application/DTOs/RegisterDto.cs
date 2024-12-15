using System.Text.Json.Serialization;
using BankingApp.Domain.Entities;

namespace BankingApp.Application.DTOs;

public class RegisterDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DateOfBirth { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
}