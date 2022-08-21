using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public class OfferService : IOfferService
    {
        private readonly UserDbContext context;
        private readonly IMapper mapper;
        private readonly MailService mailService;

        public OfferService(UserDbContext context, IMapper mapper, MailService mailService)
        {
            this.mapper = mapper;
            this.context = context;
            this.mailService = mailService;
        }

        public async Task<ApplicationResponse> Create(CreateOfferDto offer, User applicationUser)
        {
            try
            {
                Product product = context.Products.Where(p => p.ProductId == offer.ProductId).FirstOrDefault();
                if (product.isOfferable == true)
                {
                    Offer newOffer = new Offer();
                    newOffer.CreatedById = applicationUser.Id;
                    newOffer.CreatedBy = applicationUser.UserName;
                    newOffer.CreatedDate = DateTime.UtcNow;
                    newOffer.ProductId = offer.ProductId;
                    newOffer.Amount = offer.Amount;
                    newOffer.isConfirmed = false;
                    newOffer.isActive = true;
                    context.Offers.Add(newOffer);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return new ApplicationResponse { Succeeded = false, ErrorMessage = "This product is closed to offer" };
                }

                return new ApplicationResponse { Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> Delete(int Id, User applicationUser)
        {
            try
            {
                Offer willDelete = await context.Offers.FindAsync(Id);
                if (willDelete != null)
                {
                    context.Offers.Remove(willDelete);
                    await context.SaveChangesAsync();

                    return new ApplicationResponse { Succeeded = true };
                }
                else
                {
                    return new ApplicationResponse { Succeeded = false, ErrorMessage = "Record not found. Try Again." };

                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<OfferDto>> GetById(int Id, User applicationUser)
        {
            try
            {
                Offer offer = await context.Offers.FindAsync(Id);
                if (offer != null)
                {
                    OfferDto dto = mapper.Map<Offer, OfferDto>(offer);
                    return new ApplicationResponse<OfferDto>
                    {
                        Result = dto,
                        Succeeded = true
                    };
                }
                else
                {
                    return new ApplicationResponse<OfferDto>{ Succeeded = false, ErrorMessage = "Record not found. Try Again." };

                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponse<OfferDto> { Succeeded = false, ErrorMessage = ex.Message };
            }

        }

        public async Task<ApplicationResponse<List<OfferDto>>> GetAll(User applicationUser)
        {
            try
            {
                List<Offer> result = await context.Offers.ToListAsync();
                List<OfferDto> mapResult = mapper.Map<List<Offer>, List<OfferDto>>(result);

                return new ApplicationResponse<List<OfferDto>>
                {
                    Result = mapResult,
                    Succeeded = true
                };

            }
            catch (Exception ex)
            {
                return new ApplicationResponse<List<OfferDto>> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<List<OfferDto>>> GetActiveOffers(User applicationUser)
        {
            try
            {
                List<Offer> result = await context.Offers.Where(x => x.isActive == true).ToListAsync();
                List<OfferDto> mapResult = mapper.Map<List<Offer>, List<OfferDto>>(result);

                return new ApplicationResponse<List<OfferDto>>
                {
                    Result = mapResult,
                    Succeeded = true
                };

            }
            catch (Exception ex)
            {
                return new ApplicationResponse<List<OfferDto>> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<List<OfferDto>>> GetSendedOffers(User applicationUser)
        {
            try
            {
                List<Offer> result = await context.Offers.Where(x => x.CreatedBy == applicationUser.Email && x.isActive == true).ToListAsync();
                List<OfferDto> mapResult = mapper.Map<List<Offer>, List<OfferDto>>(result);

                return new ApplicationResponse<List<OfferDto>>
                {
                    Result = mapResult,
                    Succeeded = true
                };

            }
            catch (Exception ex)
            {
                return new ApplicationResponse<List<OfferDto>> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<List<OfferDto>>> GetReceivedOffers(User applicationUser)
        {
            try
            {
                var result = from offer in context.Offers
                             join product in context.Products on offer.ProductId equals product.ProductId

                             select new OfferDto
                             {
                                 OfferId = offer.OfferId,
                                 ProductId = offer.ProductId,
                                 Amount = offer.Amount,
                                 isActive = offer.isActive,
                                 isConfirmed = offer.isConfirmed,
                                 CreatedDate = offer.CreatedDate,
                                 CreatedBy = product.CreatedBy,
                                 CreatedById = offer.CreatedById,
                                 ModifiedBy = offer.ModifiedBy,
                                 ModifiedById = offer.ModifiedById,
                                 ModifiedDate = offer.ModifiedDate
                             };
                var listResult = await result.Where(x => x.CreatedBy == applicationUser.Email && x.isActive == true).ToListAsync();

                return new ApplicationResponse<List<OfferDto>>
                {
                    Result = listResult,
                    Succeeded = true
                };

            }
            catch (Exception ex)
            {
                return new ApplicationResponse<List<OfferDto>> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> Update(UpdateOfferDto input, User applicationUser)
        {
            try
            {
                Offer getExistCategory = await context.Offers.FindAsync(input.OfferId);
                getExistCategory.ProductId = input.ProductId;
                getExistCategory.Amount = input.Amount;
                getExistCategory.ModifiedBy = applicationUser.UserName;
                getExistCategory.ModifiedById = applicationUser.Id;
                getExistCategory.ModifiedDate = DateTime.UtcNow;

                context.Update(getExistCategory);
                await context.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Succeeded = true,
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }

        }

        public async Task<ApplicationResponse> ConfirmOffer(int offerId, User applicationUser)
        {
            try
            {
                Offer getExistCategory = await context.Offers.FindAsync(offerId);
                getExistCategory.isConfirmed = true;

                context.Update(getExistCategory);
                await context.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Succeeded = true,
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }

        }

        public async Task<ApplicationResponse> MakeOffer(CreateOfferDto offer, User applicationUser)
        {
            try
            {
                Product product = await context.Products.Where(x => x.ProductId == offer.ProductId).FirstOrDefaultAsync();
                if (product.isOfferable != true)
                {
                    return new ApplicationResponse { Succeeded = false, ErrorMessage = "Maalsef bu urun tekliflere acik degil." };
                }
                Offer newOffer = new Offer();
                newOffer.CreatedById = applicationUser.Id;
                newOffer.CreatedBy = applicationUser.UserName;
                newOffer.CreatedDate = DateTime.UtcNow;
                newOffer.ProductId = offer.ProductId;
                newOffer.isConfirmed = false;
                newOffer.isActive = true;

                if (offer.Amount != null)
                {
                    newOffer.Amount = offer.Amount;
                }
                else if (offer.Percentage != null)
                {
                    newOffer.Amount = product.Price * offer.Percentage;
                }
                else
                {
                    return new ApplicationResponse { Succeeded = false, ErrorMessage = "Please enter a valid number or percentage !" };
                }

                context.Offers.Add(newOffer);
                await context.SaveChangesAsync();

                return new ApplicationResponse { Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> CancelOffer(int offerId, User applicationUser)
        {
            try
            {
                Offer getExistOffer = await context.Offers.FindAsync(offerId);
                getExistOffer.isActive = false;

                context.Update(getExistOffer);
                await context.SaveChangesAsync();

                //teklifi iptal eden kullanici teklifi yapan kullaniciyla ayni mi kontrol edilir
                //degilse teklifi olusturan kullanici bilgilendirilir.
                if (applicationUser.Email != getExistOffer.CreatedBy)
                {
                    Product getExistProduct = await context.Products.FindAsync(getExistOffer.ProductId);
                    var toEmail = getExistProduct.CreatedBy;

                    var tempMail = mailService.createOfferSoldMail(toEmail).Result;
                    Email toSendMail = tempMail.Result;
                    await mailService.sendMail(toSendMail);
                }

                return new ApplicationResponse
                {
                    Succeeded = true
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> BuyOffer(int offerId, User applicationUser)
        {
            try
            {
                Offer getExistOffer = await context.Offers.FindAsync(offerId);
                if (getExistOffer == null)
                {
                    return new ApplicationResponse { Succeeded = false, ErrorMessage = "No Post Found !" };
                }
                if (getExistOffer.isConfirmed == false)
                {
                    return new ApplicationResponse { Succeeded = false, ErrorMessage = "This offer is not confirmed yet !" };
                }

                // offer artik aktif degil
                getExistOffer.isActive = false;
                getExistOffer.ModifiedBy = applicationUser.UserName;
                getExistOffer.ModifiedById = applicationUser.Id;
                getExistOffer.ModifiedDate = DateTime.UtcNow;

                context.Update(getExistOffer);

                //urun durumu satildi olarak guncellenir
                Product getExistProduct = context.Products.Where(x => x.ProductId == getExistOffer.ProductId).FirstOrDefault(); 
                getExistProduct.isSold = true;
                getExistProduct.isOfferable = false;

                context.Update(getExistProduct);

                //satis satis tablosuna kaydedilir.
                Sale newSale = new Sale();
                newSale.CreatedBy = applicationUser.UserName;
                newSale.CreatedById = applicationUser.Id;
                newSale.CreatedDate = DateTime.UtcNow;
                newSale.Amount = getExistOffer.Amount;
                newSale.ProductId = getExistProduct.ProductId;

                await context.Sales.AddAsync(newSale);

                await context.SaveChangesAsync();

                // teklifin aktifligi dusup satis onaylaninca urun koduna ait diger teklifler de iptal edilir.
                List<Offer> offerList = context.Offers.Where(x => x.ProductId == getExistOffer.ProductId).ToList();

                // cancelOffer methodu satilan urune teklif vermis kullanicilara tekliflerinin artik aktif olmadigi
                // konusunda bir mail gonderir
                foreach (Offer offer in offerList)
                {
                    await CancelOffer(offer.OfferId, applicationUser);
                }

                //urun sahibine urununuz satildi maili
                var ownerOfProduct = getExistProduct.CreatedBy;
                var tempMail = mailService.createSaleMail(ownerOfProduct).Result;
                Email toSendMail = tempMail.Result;
                await mailService.sendMail(toSendMail);

                return new ApplicationResponse { Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> BuyProduct(int productId, User applicationUser)
        {
            try
            {
                Product getExistProduct = await context.Products.FindAsync(productId);

                if (getExistProduct == null)
                {
                    return new ApplicationResponse { Succeeded = false, ErrorMessage = "No Post Found !" };
                }

                getExistProduct.isSold = true;
                getExistProduct.isOfferable = false;
                getExistProduct.ModifiedBy = applicationUser.UserName;
                getExistProduct.ModifiedById = applicationUser.Id;
                getExistProduct.ModifiedDate = DateTime.UtcNow;

                context.Update(getExistProduct);

                Sale newSale = new Sale();
                newSale.CreatedBy = applicationUser.UserName;
                newSale.CreatedById = applicationUser.Id;
                newSale.CreatedDate = DateTime.UtcNow;
                newSale.Amount = getExistProduct.Price;
                newSale.ProductId = getExistProduct.ProductId;

                await context.Sales.AddAsync(newSale);

                await context.SaveChangesAsync();

                List<Offer> offerList = context.Offers.Where(x => x.ProductId == productId).ToList();

                //urun satin alindiginda o urune yapilmis teklifler iptal edilir.
                foreach (Offer offer in offerList)
                {
                    await CancelOffer(offer.OfferId, applicationUser);
                }

                //urun sahibine urununuz satildi maili
                var ownerOfProduct = getExistProduct.CreatedBy;
                var tempMail = mailService.createSaleMail(ownerOfProduct).Result;
                Email toSendMail = tempMail.Result;
                await mailService.sendMail(toSendMail);

                return new ApplicationResponse { Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }
    }
}
