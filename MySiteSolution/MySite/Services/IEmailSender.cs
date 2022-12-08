﻿using System.Threading.Tasks;

namespace MyCatalogSite.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}