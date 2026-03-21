using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    public void SendEmail(string toEmail, string message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("Localdelivery16@gmail.com"));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Order Confirmation";

        email.Body = new TextPart("plain")
        {
            Text = message
        };

        using var smtp = new SmtpClient();

        smtp.Connect("smtp.gmail.com", 587, false);

        smtp.Authenticate("Localdelivery16@gmail.com", "123@delivery#");

        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
