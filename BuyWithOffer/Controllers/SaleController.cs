using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    [Route("[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        IProductService productService;

        public SaleController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<ApplicationResponse<List<Sale>>> GetAll([FromBody] CreateCategoryDto input)
        {
            return await productService.GetAllSales();
        }
    }
}
