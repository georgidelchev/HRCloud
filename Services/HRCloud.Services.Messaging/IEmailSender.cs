﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCloud.Services.Messaging
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null);
    }
}
