using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System;

namespace Services
{
    public class EmailService
    {
        private  EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<int> Send(Mail model)
        {
            var smtpClient = new SmtpClient
            {
                Host = _emailSettings.MailServer, // set your SMTP server name here
                Port = _emailSettings.MailPort, // Port 
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password)
            };

            using (var message = new MailMessage(_emailSettings.SenderEmail, model.Email)
            {
                Subject = model.Subject,
                Body = model.Message,
                IsBodyHtml = true,
                Priority = MailPriority.Normal,
            })
            {
                // message.CC.Add("mohamed.mourabit@outlook.com");
                await smtpClient.SendMailAsync(message);

                return 1;
            }
        }

        public async Task<string> SendEmailAsync(string emails, string subject, string message)
        {
            var smtpClient = new SmtpClient
            {
                Host = _emailSettings.MailServer, // set your SMTP server name here
                Port = _emailSettings.MailPort, // Port 
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password),
                Timeout = 30000
            };

            MailAddress from = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
            // MailAddress to = new MailAddress(email, email);
            // MailMessage to = new MailMessage();



            // MailMessage message = new MailMessage();
            // message.To.Add(new MailAddress("email1@evilcorp.com"));
            // message.To.Add(new MailAddress("email2@evilcorp.com"));

            using (var Message = new MailMessage()
            {
                From = from,
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
                Priority = MailPriority.Normal,
            })
            {
                // message.CC.Add("mohamed.mourabit@outlook.com");
                try
                {
                    foreach (var email in emails.Split(';'))
                    {
                        if (email != "")
                        {
                            Message.To.Add(new MailAddress(email, email));
                        }
                    }

                    await smtpClient.SendMailAsync(Message);
                }
                catch (System.Exception e)
                {
                    return e.Message;
                }


                return "ok";
            }
        }


    }
}