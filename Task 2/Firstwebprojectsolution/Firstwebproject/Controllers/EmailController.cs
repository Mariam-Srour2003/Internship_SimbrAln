using Firstwebproject; // Adjust the namespace according to your project structure
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class EmailController : Controller
{
    private readonly EmailService _emailService;

    public EmailController(EmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<IActionResult> SendEmail()
    {
        // Logic to generate email content, recipient, and subject
        string toEmail = "recipient@example.com";
        string subject = "Email Subject";
        string message = "Email Message";

        await _emailService.SendEmailAsync(toEmail, subject, message);

        return RedirectToAction("Index"); // Redirect to some other page
    }
}
