using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;

        public ProductController(IProductService productService, UserManager<User> userManager)
        {
            this.productService = productService;
            this.userManager = userManager;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<ApplicationResponse<List<ProductDto>>> Get()
        {
            Log.Information($"{User.Identity?.Name}: get all products");
            var user = await GetCurrentUserAsync();
            return await productService.GetAll(user);
        }

        [HttpGet("GetOpenForSale")]
        [Authorize]
        public async Task<ApplicationResponse<List<ProductDto>>> GetOpenForSale()
        {
            Log.Information($"{User.Identity?.Name}: get products which open for sale");
            var user = await GetCurrentUserAsync();
            return await productService.GetOpenForSale(user);
        }

        [HttpGet("GetOpenToOffer")]
        [Authorize]
        public async Task<ApplicationResponse<List<ProductDto>>> GetOpenToOffer()
        {
            Log.Information($"{User.Identity?.Name}: get products which open for offer");
            var user = await GetCurrentUserAsync();
            return await productService.GetOpenToOffer(user);
        }

        [HttpGet("GetById")]
        [Authorize]
        public async Task<ActionResult<ApplicationResponse<ProductDto>>> GetById(int id)
        {
            Log.Information($"{User.Identity?.Name}: get a product with id is {id}");
            var user = await GetCurrentUserAsync();
            var result = await productService.GetById(id, user);
            if (result.Succeeded)
                return result;

            return NotFound(result);
        }

        [HttpGet("GetByUser")]
        [Authorize]
        public async Task<ActionResult<ApplicationResponse<List<ProductDto>>>> GetByUser()
        {
            Log.Information($"{User.Identity?.Name}: get products which are belong active user");
            var user = await GetCurrentUserAsync();
            var result = await productService.GetByUser(user);
            if (result.Succeeded)
                return result;

            return NotFound(result);
        }

        [HttpGet("GetByCat")]
        [Authorize]
        public async Task<ApplicationResponse<List<ProductDto>>> GetByCat([FromQuery] string category)
        {
            Log.Information($"{User.Identity?.Name}: get products by a category");
            return await productService.GetByCat(category);
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult<ApplicationResponse>> Create([FromBody] CreateProductDto input)
        {
            Log.Information($"{User.Identity?.Name}: create product which is {input.ProductName}");
            var user = await GetCurrentUserAsync();
            var result = await productService.Create(input, user);
            if (result.Succeeded)
                return result;
            return NotFound(result);
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<ApplicationResponse> Update([FromBody] UpdateProductDto request)
        {
            Log.Information($"{User.Identity?.Name}: update product with id is {request.ProductId}");
            var user = await GetCurrentUserAsync();
            var result = await productService.Update(request, user);
            return result;
        }

        [HttpPut("OpenToOffer")]
        [Authorize]
        public async Task<ApplicationResponse> OpenToOffer(int productId)
        {
            Log.Information($"{User.Identity?.Name}: opened product to offer with id is {productId}");
            var user = await GetCurrentUserAsync();
            var result = await productService.OpenToOffer(productId, user);
            return result;
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<ApplicationResponse> Delete(int productId)
        {
            Log.Information($"{User.Identity?.Name}: deleted product to offer with id is {productId}");
            var user = await GetCurrentUserAsync();
            var result = await productService.Delete(productId, user);
            return result;
        }

        private Task<User> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet("GetColors")]
        [Authorize]
        public Task<ApplicationResponse<List<Color>>> GetColors()
        {
            return productService.GetColors();
        }

        [HttpGet("GetBrands")]
        [Authorize]
        public Task<ApplicationResponse<List<Brand>>> GetBrands()
        {
            return productService.GetBrands();
        }

        [HttpGet("GetUsageStatus")]
        [Authorize]
        public Task<ApplicationResponse<List<UsageStatus>>> GetUsageStatus()
        {
            return productService.GetUsageStatus();
        }

        [HttpPost("AddImage")]
        [Authorize]
        public async Task<ActionResult<ApplicationResponse>> AddImage([FromForm(Name = "image")] FileUpload file, int productId)
        {
            Log.Information($"{User.Identity?.Name}: add image to product with id is {productId}");
            var image = productService.GetBytes(file.image).Result;
            var user = await GetCurrentUserAsync();
            var result = await productService.AddImage(image, productId, user);
            if (result.Succeeded)
                return result;
            return NotFound(result);
        }


        [HttpPut("DeleteImage")]
        [Authorize]
        public async Task<ApplicationResponse> DeleteImage(int imageId)
        {
            Log.Information($"{User.Identity?.Name}: delete image from product with image id is {imageId}");
            var user = await GetCurrentUserAsync();
            var result = await productService.DeleteImage(imageId, user);
            return result;
        }
        
    }
}
