using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public interface ICRUDService<MainDto, CreateDto, UpdateDto, ApplicationUser>
    {
        Task<ApplicationResponse<MainDto>> GetById(int id, ApplicationUser applicationUser);
        Task<ApplicationResponse<List<MainDto>>> GetAll(ApplicationUser applicationUser);
        Task<ApplicationResponse> Create(CreateDto input, ApplicationUser applicationUser);
        Task<ApplicationResponse> Update(UpdateDto input, ApplicationUser applicationUser);
        Task<ApplicationResponse> Delete(int id, ApplicationUser applicationUser);
    }
}
