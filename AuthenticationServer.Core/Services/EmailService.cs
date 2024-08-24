using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Core.Services
{
    public class EmailService
    {
        public static async Task Send(string To, string Subject, string Body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("zolfisahand1386@gmail.com");
                mail.To.Add(To);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("zolfisahand1386@gmail.com", "yxqrmamhbbdlyzcf");
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
        }
    }
}
