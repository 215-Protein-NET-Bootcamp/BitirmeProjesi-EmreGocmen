using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public class CategoryService : ICategoryService
    {
        private readonly UserDbContext context;
        protected readonly IUnitOfWork UnitOfWork;

        public CategoryService(UserDbContext context, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.UnitOfWork = unitOfWork;
        }

        public async Task<ApplicationResponse> Create(CreateCategoryDto input, User applicationUser)
        {
            try
            {
                Category newCategory = new Category();
                newCategory.CategoryName = input.Category;
                newCategory.CreatedById = applicationUser.Id;
                newCategory.CreatedBy = applicationUser.UserName;
                newCategory.CreatedDate = DateTime.UtcNow;

                context.Categories.Add(newCategory);
                await UnitOfWork.CompleteAsync();

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
                Category willDelete = await context.Categories.FindAsync(Id);
                // kategoriyi olusturan kullanici ile aktif kullanicinin eslestigi kontrol edilir.
                if (willDelete.CreatedBy == applicationUser.UserName)
                {
                    if (willDelete != null)
                    {
                        context.Categories.Remove(willDelete);
                        await UnitOfWork.CompleteAsync();

                        return new ApplicationResponse { Succeeded = true };
                    }
                    else
                    {
                        return new ApplicationResponse { Succeeded = false, ErrorMessage = "Kayit bulunamadi." };

                    }
                }
                else
                {
                    return new ApplicationResponse { Succeeded = false,
                        ErrorMessage = "Silmek istediginiz kategory sizin tarafinizdan olusturulmadi" };

                }

            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<CategoryDto>> GetById(int Id, User applicationUser)
        {
            try
            {
                Category category = await context.Categories.FindAsync(Id);
                CategoryDto categoryDto = new CategoryDto();
                categoryDto.Category = category.CategoryName;
                categoryDto.CategoryId = category.CategoryId;

                categoryDto.CreatedBy = category.CreatedBy;
                categoryDto.CreatedById = category.CreatedById;
                categoryDto.CreatedDate = category.CreatedDate;
                categoryDto.ModifiedBy = category.ModifiedBy;
                categoryDto.ModifiedById = category.ModifiedById;
                categoryDto.ModifiedDate = category.ModifiedDate;
                return new ApplicationResponse<CategoryDto>
                {
                    Result = categoryDto,
                    Succeeded = true
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse<CategoryDto> { Succeeded = false, ErrorMessage = ex.Message };
            }

        }

        public async Task<ApplicationResponse<List<CategoryDto>>> GetAll(User applicationUser)
        {
            try
            {
                List<Category> listRaw = await context.Categories.ToListAsync();
                List<CategoryDto> list = new List<CategoryDto>();
                foreach (var category in listRaw)
                {
                    CategoryDto categoryDto = new CategoryDto();
                    categoryDto.Category = category.CategoryName;
                    categoryDto.CategoryId = category.CategoryId;

                    categoryDto.CreatedBy = category.CreatedBy;
                    categoryDto.CreatedById = category.CreatedById;
                    categoryDto.CreatedDate = category.CreatedDate;
                    categoryDto.ModifiedBy = category.ModifiedBy;
                    categoryDto.ModifiedById = category.ModifiedById;
                    categoryDto.ModifiedDate = category.ModifiedDate;
                    list.Add(categoryDto);
                }
                return new ApplicationResponse<List<CategoryDto>>
                {
                    Result = list,
                    Succeeded = true
                };

            }
            catch (Exception ex)
            {
                return new ApplicationResponse<List<CategoryDto>> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> Update(UpdateCategoryDto input, User applicationUser)
        {
            try
            {
                Category getExistCategory = await context.Categories.FindAsync(input.CategoryId);
                // kategoriyi olusturan kullanici ile aktif kullanicinin eslestigi kontrol edilir.
                if (getExistCategory.CreatedBy == applicationUser.UserName)
                {
                    if (getExistCategory != null)
                    {
                        getExistCategory.CategoryName = input.Category;
                        getExistCategory.ModifiedBy = applicationUser.UserName;
                        getExistCategory.ModifiedById = applicationUser.Id;
                        getExistCategory.ModifiedDate = DateTime.UtcNow;

                        context.Update(getExistCategory);
                        await UnitOfWork.CompleteAsync();

                        return new ApplicationResponse
                        { Succeeded = true };
                    }
                    else
                    {
                        return new ApplicationResponse { Succeeded = false, ErrorMessage = "Kayit bulunamadi." };

                    }

                }
                else
                {
                    return new ApplicationResponse { Succeeded = false,
                        ErrorMessage = "Guncellemek istediginiz kategori sizin tarafinizdan olusturulmadi" };
                }

            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }

        }
    }
}
