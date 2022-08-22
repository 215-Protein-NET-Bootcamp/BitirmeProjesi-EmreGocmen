using Xunit;

namespace BuyWithOffer.Test
{   
    // Diger servisler database ile baglantili calistigi icin unit tesleri sadece mail servisi icin yazdim.
    public class MailServiceTest : IClassFixture<MailService>
    {
        MailService mailService;
        private readonly UserDbContext context;
        protected readonly IUnitOfWork UnitOfWork;
        public MailServiceTest(UserDbContext context, IUnitOfWork unitOfWork, MailService mailService)
        {
            this.mailService = mailService;
            this.context = context;
            this.UnitOfWork = unitOfWork;
        }

        [Fact]
        public void CreateWelcomeMailTest()
        {
            Email testMail = new Email();
            testMail.Subject = "Hosgeldiniz";
            testMail.From = "buywithoffer@outlook.com";
            testMail.To = "emre.678@hotmail.com";
            testMail.Body = "Hesabiniz olusturuldu. Keyifle tekliflerinizi yapiniz!";
            testMail.Status = "Created";
            testMail.tryCount = 1;

            var tempMail = mailService.createWelcomeMail("emre.678@hotmail.com").Result;
            var testMail2 = tempMail.Result;

            Assert.Equal(testMail, testMail2);
        }
    }
}
