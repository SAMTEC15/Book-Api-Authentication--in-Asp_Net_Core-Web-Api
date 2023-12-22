using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Application.Interfaces
{
    public interface IEmailService
    {
        //Task SendEmailAsync(MailRequest mailRequest);
        Task SendHtmlEmailAsync(MailRequest request);
    }
}
