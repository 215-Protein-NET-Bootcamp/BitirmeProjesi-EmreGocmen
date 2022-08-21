using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    [Route("[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService offerService;
        private readonly UserManager<User> userManager;
        public OfferController(IOfferService offerService, UserManager<User> userManager)
        {
            this.offerService = offerService;
            this.userManager = userManager;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<ApplicationResponse<List<OfferDto>>> GetAll()
        {
            Log.Information($"{User.Identity?.Name}: get all offers");
            var user = await GetCurrentUserAsync();
            return await offerService.GetAll(user);
        }

        [HttpGet("GetActiveOffers")]
        [Authorize]
        public async Task<ApplicationResponse<List<OfferDto>>> GetActiveOffers()
        {
            Log.Information($"{User.Identity?.Name}: get active offers");
            var user = await GetCurrentUserAsync();
            return await offerService.GetActiveOffers(user);
        }

        [HttpGet("GetById")]
        [Authorize]
        public async Task<ActionResult<ApplicationResponse<OfferDto>>> GetById(int id)
        {
            Log.Information($"{User.Identity?.Name}: get an offer by id with id is {id}");
            var user = await GetCurrentUserAsync();
            var result = await offerService.GetById(id, user);
            if (result.Succeeded)
                return result;
            return NotFound(result);
        }

        [HttpGet("GetSendedOffers")]
        [Authorize]
        public async Task<ApplicationResponse<List<OfferDto>>> GetSendedOffers()
        {
            Log.Information($"{User.Identity?.Name}: get sended offers by a user");
            var user = await GetCurrentUserAsync();
            return await offerService.GetSendedOffers(user);
        }

        [HttpGet("GetReceivedOffers")]
        [Authorize]
        public async Task<ApplicationResponse<List<OfferDto>>> GetReceivedOffers()
        {
            Log.Information($"{User.Identity?.Name}: get received offers");
            var user = await GetCurrentUserAsync();
            return await offerService.GetReceivedOffers(user);
        }

        [HttpPost("MakeOffer")]
        [Authorize]
        public async Task<ActionResult<ApplicationResponse>> MakeOffer([FromBody] CreateOfferDto input)
        {
            Log.Information($"{User.Identity?.Name}: to make offer creates new offer for product which is {input.ProductId}");
            var user = await GetCurrentUserAsync();
            var result = await offerService.MakeOffer(input, user);
            if (result.Succeeded)
                return result;

            return NotFound(result);
        }

        [HttpPut("ConfirmOffer")]
        [Authorize]
        public async Task<ApplicationResponse> ConfirmOffer(int offerId)
        {
            Log.Information($"{User.Identity?.Name}: to confirm an offer updates offer table on database with id is {offerId}");
            var user = await GetCurrentUserAsync();
            var result = await offerService.ConfirmOffer(offerId, user);
            return result;
        }

        [HttpPut("CancelOffer")]
        [Authorize]
        public async Task<ApplicationResponse> CancelOffer(int offerId)
        {
            Log.Information($"{User.Identity?.Name}: to cancel an offer updates offer table on database with id is {offerId}");
            var user = await GetCurrentUserAsync();
            var result = await offerService.CancelOffer(offerId, user);
            return result;
        }

        [HttpPut("BuyOffer")]
        [Authorize]
        public async Task<ApplicationResponse> BuyOffer(int offerId)
        {
            Log.Information($"{User.Identity?.Name}: to buy an offer updates offer table on database with id is {offerId}");
            var user = await GetCurrentUserAsync();
            var result = await offerService.BuyOffer(offerId, user);
            return result;
        }

        [HttpPut("BuyProduct")]
        [Authorize]
        public async Task<ApplicationResponse> BuyProduct(int productId)
        {
            Log.Information($"{User.Identity?.Name}: to buy a product without offer creates new sale to sale table on database with id is {productId}");
            var user = await GetCurrentUserAsync();
            var result = await offerService.BuyProduct(productId, user);
            return result;
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<ApplicationResponse> Delete(int offerId)
        {
            Log.Information($"{User.Identity?.Name}: delete an offer with id is {offerId}");
            var user = await GetCurrentUserAsync();
            var result = await offerService.Delete(offerId, user);
            return result;
        }

        private Task<User> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
    }
}
