using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public interface IProductService : ICRUDService<ProductDto, CreateProductDto, UpdateProductDto, User>
    {
        Task<ApplicationResponse<List<ProductDto>>> GetByCat(string category);
        Task<ApplicationResponse<List<ProductDto>>> GetByUser(User applicationUser);
        Task<ApplicationResponse<List<ProductDto>>> GetOpenForSale(User applicationUser);
        Task<ApplicationResponse<List<ProductDto>>> GetOpenToOffer(User applicationUser);
        Task<ApplicationResponse<List<Color>>> GetColors();
        Task<ApplicationResponse<List<Brand>>> GetBrands();
        Task<ApplicationResponse<List<UsageStatus>>> GetUsageStatus();
        Task<ApplicationResponse> OpenToOffer(int productId, User applicationUser);
        Task<ApplicationResponse> DeleteImage(int imageId, User applicationUser);
        Task<ApplicationResponse> AddImage(byte[] image, int productId, User applicationUser);
        Task<byte[]> GetBytes(IFormFile formFile);
    }
}
