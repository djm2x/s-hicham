using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;

namespace Services
{
    public class AccountService
    {
        private  EmailService emailService;
        private  IWebHostEnvironment _env;
        public AccountService(EmailService emailService, IWebHostEnvironment env)
        {
            this.emailService = emailService;
            _env = env;
        }
        public async Task<string> GenerateLinkForEmailAsync(
            HttpRequest request
            , string returnUrl
            , string email
            , string subject = "Operation"
            , string message = ""
            )
        {
            returnUrl = returnUrl.Replace("%2F", "/");
            var codeBasic = $"{email}*{DateTime.Now}";
            // generate code
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codeBasic));
            // create link to be send to email
            var callbackUrl = $"{request.Scheme}://{request.Host}{request.PathBase}/{returnUrl}/{code}";
            
            if (_env.IsDevelopment())
            {
                callbackUrl = $"http://localhost:4200/{returnUrl}/{code}";
            }
            // edit message by adding link clickeble
            message += $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clicke ici</a>";
            
            // send the email
            await emailService.SendEmailAsync(email, subject, message);

            return codeBasic;
        }

        public string GetUrl(HttpRequest request, string sub = "")
        {
            // create link to be send to email
            var url = $"{request.Scheme}://{request.Host}{request.PathBase}/{sub}";
            
            if (_env.IsDevelopment())
            {
                url = $"http://localhost:4202/{sub}";
            }

            return url;
        }


         public string GenerateActivationLink(
            HttpRequest request
            , string returnUrl
            , string codeBasic 
            )
        {
            returnUrl = returnUrl.Replace("%2F", "/");
            // generate code
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codeBasic));
            // create link to be send to email
            var callbackUrl = $"{request.Scheme}://{request.Host}{request.PathBase}/{returnUrl}/{code}";
            
            if (_env.IsDevelopment())
            {
                callbackUrl = $"http://localhost:4202/{returnUrl}/{code}";
            }
            // edit message by adding link clickeble
           
            
            // send the email
            

            return callbackUrl;
        }

        

    }
}