using Microsoft.AspNetCore.Http;

namespace UMS.Infrastructure.Abstraction.EmailServiceInterface;

public interface IEEmailService
{
    Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null);
}