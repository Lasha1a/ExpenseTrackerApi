using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.EmailSender;

public class EmailSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set;}
    public string SenderEmail { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public string AppPassword { get; set; } = null!;

}
