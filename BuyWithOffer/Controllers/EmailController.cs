using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        MailService mailService;

        public EmailController(MailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPut("SendFailedMails")]
        [Authorize]
        public async Task<ApplicationResponse> SendFailedMails()
        {
            return await mailService.sendFailedMails();
        }
    }
}
