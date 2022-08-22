using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public class MailService
    {
        private readonly UserDbContext context;
        protected readonly IUnitOfWork UnitOfWork;

        public MailService(UserDbContext context, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.UnitOfWork = unitOfWork;
        }

        public async Task<ApplicationResponse<Email>> createWelcomeMail(string emailAddress)
        {
            try
            {
                Email mail = new Email();
                mail.Subject = "Hosgeldiniz";
                mail.From = "buywithoffer@outlook.com";
                mail.To = emailAddress;
                mail.Body = "Hesabiniz olusturuldu. Keyifle tekliflerinizi yapiniz!";
                mail.Status = "Created";
                mail.tryCount = 1;

                context.Email.Add(mail);
                await UnitOfWork.CompleteAsync();

                return new ApplicationResponse<Email> { Result = mail, Succeeded = true };
            }
            catch(Exception ex)
            {
                return new ApplicationResponse<Email> { Succeeded = false, ErrorMessage = ex.Message };
            }     
        }

        public async Task<ApplicationResponse<Email>> createBlockedMail(string emailAddress)
        {
            try
            {
                Email mail = new Email();
                mail.Subject = "Hesap Bloke";
                mail.From = "buywithoffer@outlook.com";
                mail.To = emailAddress;
                mail.Body = "Parolaniz 3 kez yanlis girildigi icin hesabiniz bloke olmustur. 5 dakika sonra tekrar deneyebilirsiniz.";
                mail.Status = "Created";
                mail.tryCount = 1;

                context.Email.Add(mail);
                await UnitOfWork.CompleteAsync();

                return new ApplicationResponse<Email> { Result = mail ,Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse<Email> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<Email>> createSaleMail(string emailAddress)
        {
            try
            {
                Email mail = new Email();
                mail.Subject = "Urunuz satilmistir";
                mail.From = "buywithoffer@outlook.com";
                mail.To = emailAddress;
                mail.Body = "Urununuz basariyla satilmistir, yeni teklifler almak ve gondermek icin sitemize bekleriz.";
                mail.Status = "Created";
                mail.tryCount = 1;

                context.Email.Add(mail);
                await UnitOfWork.CompleteAsync();

                return new ApplicationResponse<Email> { Result = mail, Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse<Email> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<Email>> createOfferSoldMail(string emailAddress)
        {
            try
            {
                Email mail = new Email();
                mail.Subject = "Teklif verdiginiz urun satilmistir";
                mail.From = "buywithoffer@outlook.com";
                mail.To = emailAddress;
                mail.Body = "Maalesef teklif verdiginiz urun satilmistir," +
                    " yeni teklifler almak ve gondermek icin sitemize bekleriz.";
                mail.Status = "Created";
                mail.tryCount = 1;

                context.Email.Add(mail);
                await UnitOfWork.CompleteAsync();

                return new ApplicationResponse<Email> { Result = mail, Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse<Email> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }


        // olusturulan mailleri gondermek icin bu metod cagirilir.
        public async Task<ApplicationResponse> sendMail(Email mail)
        {
            MailMessage message = new MailMessage();
            message.Subject = mail.Subject;
            message.From = new MailAddress("yeniepostabwo@outlook.com", "BuyWithOffer");
            message.To.Add(new MailAddress(mail.To));
            message.Body = mail.Body;

            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            NetworkCredential AccountInfo = new NetworkCredential("yeniepostabwo@outlook.com", "Asd123.:.");
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = AccountInfo;
            try
            {
                smtp.Send(message);
                mail.Status = "Sended";
                context.Update(mail);
                await context.SaveChangesAsync();
                return new ApplicationResponse { Succeeded = true };
            }
            catch (Exception ex)
            {
                if (mail.tryCount <= 5)
                {
                    mail.tryCount += 1;
                    context.Update(mail);
                    await UnitOfWork.CompleteAsync();
                    await sendMail(mail);
                }
                mail.Status = "Failed";
                context.Update(mail);
                await UnitOfWork.CompleteAsync();

                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        // 5  defa denenip status u failed olarak guncellenen mailleri tekrar gondermeyi dener.
        public async Task<ApplicationResponse> sendFailedMails()
        {
            List<Email> failedMails = await context.Email.Where(x => x.Status == "Failed").ToListAsync();

            try
            {
                foreach(Email mail in failedMails)
                {
                    // failed mailler tekrar gonderilmeden once tryCount degerleri sifirlanir(default olarak 1).
                    // bu sayede 5 kere daha denenir.
                    mail.tryCount = 1;
                    context.Update(mail);
                    await UnitOfWork.CompleteAsync();
                    await sendMail(mail);
                }
                return new ApplicationResponse { Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }


        }

    }
}
