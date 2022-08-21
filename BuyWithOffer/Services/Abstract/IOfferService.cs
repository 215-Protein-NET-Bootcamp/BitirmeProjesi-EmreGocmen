using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public interface IOfferService : ICRUDService<OfferDto, CreateOfferDto, UpdateOfferDto, User>
    {
        Task<ApplicationResponse> MakeOffer(CreateOfferDto offer, User applicationUser);
        Task<ApplicationResponse<List<OfferDto>>> GetSendedOffers(User applicationUser);
        Task<ApplicationResponse<List<OfferDto>>> GetReceivedOffers(User applicationUser);
        Task<ApplicationResponse> ConfirmOffer(int offerId, User applicationUser);
        Task<ApplicationResponse> CancelOffer(int offerId, User applicationUser);
        Task<ApplicationResponse> BuyOffer(int offerId, User applicationUser);
        Task<ApplicationResponse<List<OfferDto>>> GetActiveOffers(User applicationUser);
        Task<ApplicationResponse> BuyProduct(int productId, User applicationUser);
    }
}
