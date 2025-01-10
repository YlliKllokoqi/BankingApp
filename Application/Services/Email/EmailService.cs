using BankingApp.Application.DTOs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BankingApp.Application.Services.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<ResultDto<string>> SendDebitCardApprovalEmail(string clientEmail)
    {
        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Banking App", emailSettings["SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(clientEmail));
            email.Subject = "Debit Card Approved";
            email.Body = new TextPart("plain")
            {
                Text =
                    "Dear client, \n we are happy to inform you that your debit card has been approved and is now ready for usage. " +
                    $"For further information please do not hesitate to contact us at {emailSettings["SenderEmail"]}. \n" +
                    "Best regards, \n" +
                    "Stella Bank"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["AppPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            
            return ResultDto<string>.Success("Debit Card approved");

        }
        catch (Exception e)
        {
            return ResultDto<string>.Failure(new ErrorResponseDto
            {
                Message = "EMAIL_SENDER_MAIL_ERROR",
                Details = e.Message
            });
        }
    }
}