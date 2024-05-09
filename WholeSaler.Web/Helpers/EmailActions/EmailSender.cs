using System;
using System.Net;
using System.Net.Mail;
using System.Text;
namespace WholeSaler.Web.Helpers.EmailActions
{
    public class EmailSender
    {
        public static void SendEMail(string email,string subject,string body)
        {
         MailMessage sender = new MailMessage();
            sender.From = new MailAddress("barisemirdag@gmail.com","Wholesaller E-Commerce");
            sender.Subject = subject;
            sender.Body = body;
            sender.BodyEncoding = Encoding.UTF8;
            sender.To.Add(email);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("barisemirdag@gmail.com", "ltrb evph belq wmyf");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            smtpClient.Send(sender);
        }
    }
}
