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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly UserManager<User> userManager;
        public CategoryController(ICategoryService categoryService, UserManager<User> userManager)
        {
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<ApplicationResponse<List<CategoryDto>>> Get()
        {
            Log.Information($"{User.Identity?.Name}: get all categories");
            var user = await GetCurrentUserAsync();
            return await categoryService.GetAll(user);
        }

        [HttpGet("GetById")]
        [Authorize]
        public async Task<ActionResult<ApplicationResponse<CategoryDto>>> GetById(int id)
        {
            Log.Information($"{User.Identity?.Name}: get a category by id with id is {id}");
            var user = await GetCurrentUserAsync();
            var result = await categoryService.GetById(id, user);
            if (result.Succeeded)
                return result;
            return NotFound(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApplicationResponse>> Post([FromBody] CreateCategoryDto input)
        {
            Log.Information($"{User.Identity?.Name}: create a category {input.Category}");
            var user = await GetCurrentUserAsync();
            var result = await categoryService.Create(input, user);
            if (result.Succeeded)
                return result;

            return NotFound(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ApplicationResponse> Update([FromBody] UpdateCategoryDto request)
        {
            Log.Information($"{User.Identity?.Name}: update a category with id is {request.CategoryId}");
            var user = await GetCurrentUserAsync();
            var result = await categoryService.Update(request, user);
            return result;
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<ApplicationResponse> Delete(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a category with id is {id}");
            var user = await GetCurrentUserAsync();
            var result = await categoryService.Delete(id, user);
            return result;
        }

        private Task<User> GetCurrentUserAsync()
        {
            Log.Information($"{User.Identity?.Name}: getting active user");
            return userManager.GetUserAsync(HttpContext.User);
        }
    }
}
