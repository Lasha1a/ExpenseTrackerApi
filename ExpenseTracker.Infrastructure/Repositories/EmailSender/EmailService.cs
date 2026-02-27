using ExpenseTracker.Application.DTOs.EmailSender;
using ExpenseTracker.Application.Interfaces.Email;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repositories.EmailSender;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    // Send email asynchronously with MailKit
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        message.To.Add(new MailboxAddress(toEmail,toEmail));
        message.Subject = subject;
        message.Body= new TextPart("plain")
        {
            Text = body
        };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.AppPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
