using EPS.Service.Dtos.Email;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class EmailService
    {
        private const string templatePath = @"C:\Users\Admin\Desktop\at_smpservice-dev\at_smpservice-dev\EPS.Service\Dtos\Email\templates\forgot.html";
        private readonly SMTPConfigModel _smtpConfig;

        public async Task SendTestEmail(UserEmailOptions userEmailOptions, string newPass)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello " + userEmailOptions.ToEmails.Split("@")[0] + " , This is mail to send password of you", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = "Your new password is : " + newPass;

            await SendEmail(userEmailOptions);
        }

        public async Task SendMailConfirm(UserEmailOptions userEmailOptions, List<string> infor)
        {
            //userEmailOptions.Subject = UpdatePlaceHolders("Hello " + userEmailOptions.ToEmails.Split("@")[0] + " , This is mail to send password of you", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = "Hello "+userEmailOptions.ToEmails +", Your new password is : " +userEmailOptions.Subject;

            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions)
        {
            //userEmailOptions.Subject = userEmailOptions.Subject;

            //userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, reset your password.", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML,
            };


            mail.To.Add(userEmailOptions.ToEmails);

            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };
            //smtpClient.UseDefaultCredentials = false;
            mail.BodyEncoding = Encoding.Default;


            await smtpClient.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
