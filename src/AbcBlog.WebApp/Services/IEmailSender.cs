using AbcBlog.Core.ConfigOptions;
using AbcBlog.WebApp.Models;

namespace AbcBlog.WebApp.Services
{
    public interface IEmailSender
    {
        Task SendEmail(EmailData emailData);
    }
}
