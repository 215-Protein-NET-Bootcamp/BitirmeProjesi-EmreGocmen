using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    public class ProductService : IProductService
    {
        private readonly UserDbContext context;
        private readonly IMemoryCache memoryCache;

        public ProductService(UserDbContext context, IMemoryCache memoryCache)
        {
            this.context = context;
            this.memoryCache = memoryCache;
        }


        public async Task<ApplicationResponse<ProductDto>> GetById(int id, User applicationUser)
        {
            try
            {
                Product product = await context.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
                ProductDto productDto = new ProductDto();

                productDto.ProductId = product.ProductId;
                productDto.ProductName = product.ProductName;
                productDto.Explanation = product.Explanation;
                productDto.Category = GetCategoryName(product.CategoryId);
                productDto.Color = GetColorName(product.ColorId);
                productDto.Brand = GetBrandName(product.BrandId);
                productDto.UsageStatus = GetUsageStatus(product.UsageStatusId);
                productDto.Image = GetImage(product);
                productDto.Price = product.Price;
                productDto.isOfferable = product.isOfferable;
                productDto.isSold = product.isSold;

                productDto.CreatedBy = product.CreatedBy;
                productDto.CreatedById = product.CreatedById;
                productDto.CreatedDate = product.CreatedDate;
                productDto.ModifiedBy = product.ModifiedBy;
                productDto.ModifiedById = product.ModifiedById;
                productDto.ModifiedDate = product.ModifiedDate;
                return new ApplicationResponse<ProductDto>
                {
                    Succeeded = true,
                    Result = productDto
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse<ProductDto> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<List<ProductDto>>> GetByUser( User applicationUser)
        {
            try
            {
                List<Product> listRaw = await context.Products.Where(x => x.CreatedById == applicationUser.Id).ToListAsync();
                List<ProductDto> list = new List<ProductDto>();
                foreach (var product in listRaw)
                {
                    ProductDto productDto = new ProductDto();
                    productDto.ProductId = product.ProductId;
                    productDto.ProductName = product.ProductName;
                    productDto.Explanation = product.Explanation;
                    productDto.Category = GetCategoryName(product.CategoryId);
                    productDto.Color = GetColorName(product.ColorId);
                    productDto.Brand = GetBrandName(product.BrandId);
                    productDto.UsageStatus = GetUsageStatus(product.UsageStatusId);
                    productDto.Image = GetImage(product);
                    productDto.Price = product.Price;
                    productDto.isOfferable = product.isOfferable;
                    productDto.isSold = product.isSold;

                    productDto.CreatedBy = product.CreatedBy;
                    productDto.CreatedById = product.CreatedById;
                    productDto.CreatedDate = product.CreatedDate;
                    productDto.ModifiedBy = product.ModifiedBy;
                    productDto.ModifiedById = product.ModifiedById;
                    productDto.ModifiedDate = product.ModifiedDate;
                    list.Add(productDto);
                }

                return new ApplicationResponse<List<ProductDto>>
                {
                    Succeeded = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse<List<ProductDto>> { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse<List<ProductDto>>> GetAll(User applicationUser)
        {
            try
            {
                List<Product> listRaw = await context.Products.ToListAsync();
                List<ProductDto> list = new List<ProductDto>();
                foreach (var product in listRaw)
                {
                    ProductDto productDto = new ProductDto();
                    productDto.ProductId = product.ProductId;
                    productDto.ProductName = product.ProductName;
                    productDto.Explanation = product.Explanation;
                    productDto.Category = GetCategoryName(product.CategoryId);
                    productDto.Color = GetColorName(product.ColorId);
                    productDto.Brand = GetBrandName(product.BrandId);
                    productDto.UsageStatus = GetUsageStatus(product.UsageStatusId);
                    productDto.Image = GetImage(product);
                    productDto.Price = product.Price;
                    productDto.isOfferable = product.isOfferable;
                    productDto.isSold = product.isSold;

                    productDto.CreatedBy = product.CreatedBy;
                    productDto.CreatedById = product.CreatedById;
                    productDto.CreatedDate = product.CreatedDate;
                    productDto.ModifiedBy = product.ModifiedBy;
                    productDto.ModifiedById = product.ModifiedById;
                    productDto.ModifiedDate = product.ModifiedDate;
                    list.Add(productDto);
                }

                return new ApplicationResponse<List<ProductDto>>
                {
                    Succeeded = true,
                    Result = list
                };
            }
            catch (Exception e)
            {
                return new ApplicationResponse<List<ProductDto>> { ErrorMessage = e.Message, Succeeded = false };
            }
        }

        public async Task<ApplicationResponse<List<ProductDto>>> GetOpenForSale(User applicationUser)
        {
            try
            {
                List<Product> listRaw = await context.Products.Where(x => x.isSold == false).ToListAsync();
                List<ProductDto> list = new List<ProductDto>();
                foreach (var product in listRaw)
                {
                    ProductDto productDto = new ProductDto();
                    productDto.ProductId = product.ProductId;
                    productDto.ProductName = product.ProductName;
                    productDto.Explanation = product.Explanation;
                    productDto.Category = GetCategoryName(product.CategoryId);
                    productDto.Color = GetColorName(product.ColorId);
                    productDto.Brand = GetBrandName(product.BrandId);
                    productDto.UsageStatus = GetUsageStatus(product.UsageStatusId);
                    productDto.Image = GetImage(product);
                    productDto.Price = product.Price;
                    productDto.isOfferable = product.isOfferable;
                    productDto.isSold = product.isSold;

                    productDto.CreatedBy = product.CreatedBy;
                    productDto.CreatedById = product.CreatedById;
                    productDto.CreatedDate = product.CreatedDate;
                    productDto.ModifiedBy = product.ModifiedBy;
                    productDto.ModifiedById = product.ModifiedById;
                    productDto.ModifiedDate = product.ModifiedDate;
                    list.Add(productDto);
                }

                return new ApplicationResponse<List<ProductDto>>
                {
                    Succeeded = true,
                    Result = list
                };
            }
            catch (Exception e)
            {
                return new ApplicationResponse<List<ProductDto>> { ErrorMessage = e.Message, Succeeded = false };
            }
        }

        public async Task<ApplicationResponse<List<ProductDto>>> GetOpenToOffer(User applicationUser)
        {
            try
            {
                List<Product> listRaw = await context.Products.Where(x => x.isOfferable == true).ToListAsync();
                List<ProductDto> list = new List<ProductDto>();
                foreach (var product in listRaw)
                {
                    ProductDto productDto = new ProductDto();
                    productDto.ProductId = product.ProductId;
                    productDto.ProductName = product.ProductName;
                    productDto.Explanation = product.Explanation;
                    productDto.Category = GetCategoryName(product.CategoryId);
                    productDto.Color = GetColorName(product.ColorId);
                    productDto.Brand = GetBrandName(product.BrandId);
                    productDto.UsageStatus = GetUsageStatus(product.UsageStatusId);
                    productDto.Image = GetImage(product);
                    productDto.Price = product.Price;
                    productDto.isOfferable = product.isOfferable;
                    productDto.isSold = product.isSold;

                    productDto.CreatedBy = product.CreatedBy;
                    productDto.CreatedById = product.CreatedById;
                    productDto.CreatedDate = product.CreatedDate;
                    productDto.ModifiedBy = product.ModifiedBy;
                    productDto.ModifiedById = product.ModifiedById;
                    productDto.ModifiedDate = product.ModifiedDate;
                    list.Add(productDto);
                }

                return new ApplicationResponse<List<ProductDto>>
                {
                    Succeeded = true,
                    Result = list
                };
            }
            catch (Exception e)
            {
                return new ApplicationResponse<List<ProductDto>> { ErrorMessage = e.Message, Succeeded = false };
            }
        }

        public async Task<ApplicationResponse<List<ProductDto>>> GetByCat(string categoryName)
        {
            try
            {
                var category = await context.Categories.Where(x => x.CategoryName == categoryName).FirstOrDefaultAsync();
                if (category != null)
                {
                    List<Product> listRaw = await context.Products.Where(p => p.CategoryId == category.CategoryId).ToListAsync();

                    if (listRaw != null)
                    {
                        List<ProductDto> list = new List<ProductDto>();
                        foreach (var product in listRaw)
                        {
                            ProductDto productDto = new ProductDto();
                            productDto.ProductId = product.ProductId;
                            productDto.ProductName = product.ProductName;
                            productDto.Explanation = product.Explanation;
                            productDto.Category = GetCategoryName(product.CategoryId);
                            productDto.Color = GetColorName(product.ColorId);
                            productDto.Brand = GetBrandName(product.BrandId);
                            productDto.UsageStatus = GetUsageStatus(product.UsageStatusId);
                            productDto.Image = GetImage(product);
                            productDto.Price = product.Price;
                            productDto.isOfferable = product.isOfferable;
                            productDto.isSold = product.isSold;

                            productDto.CreatedBy = product.CreatedBy;
                            productDto.CreatedById = product.CreatedById;
                            productDto.CreatedDate = product.CreatedDate;
                            productDto.ModifiedBy = product.ModifiedBy;
                            productDto.ModifiedById = product.ModifiedById;
                            productDto.ModifiedDate = product.ModifiedDate;
                            list.Add(productDto);
                        }
                        return new ApplicationResponse<List<ProductDto>>
                        {
                            Succeeded = true,
                            Result = list
                        };
                    }
                }
                return new ApplicationResponse<List<ProductDto>> { Succeeded = false, ErrorMessage = "No Post Found!" };

            }
            catch (Exception ex)
            {
                return new ApplicationResponse<List<ProductDto>> { Succeeded = false, ErrorMessage = ex.Message };
            }

        }

        public async Task<ApplicationResponse<List<Sale>>> GetAllSales()
        {
            try
            {
                List<Sale> listRaw = await context.Sales.ToListAsync();

                return new ApplicationResponse<List<Sale>>
                {
                    Succeeded = true,
                    Result = listRaw
                };
            }
            catch (Exception e)
            {
                return new ApplicationResponse<List<Sale>> { ErrorMessage = e.Message, Succeeded = false };
            }
        }

        public async Task<ApplicationResponse> Create(CreateProductDto input, User applicationUser)
        {
            try
            {
                Product newProduct = new Product();

                newProduct.ProductName = input.ProductName;
                newProduct.Explanation = input.Explanation;
                newProduct.CategoryId = GetCategoryId(input.Category);
                newProduct.ColorId = GetColorId(input.Color);
                newProduct.BrandId = GetBrandId(input.Brand);
                newProduct.UsageStatusId = GetUsageStatusId(input.UsageStatus);
                newProduct.Price = input.Price;
                newProduct.CreatedBy = applicationUser.UserName;
                newProduct.CreatedById = applicationUser.Id;
                newProduct.CreatedDate = DateTime.Now;

                await context.Products.AddAsync(newProduct);
                await context.SaveChangesAsync();

                return new ApplicationResponse { Succeeded = true };
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> Delete(int id, User applicationUser)
        {
            try
            {
                Product willDeleteProduct = await context.Products.FindAsync(id);
                // urun sahibi ile aktif kullanicinin eslestigi kontrol edilir
                if(willDeleteProduct.CreatedBy == applicationUser.UserName)
                {
                    if (willDeleteProduct != null)
                    {
                        context.Products.Remove(willDeleteProduct);
                        await context.SaveChangesAsync();

                        return new ApplicationResponse { Succeeded = true };
                    }
                    else
                    {
                        return new ApplicationResponse { Succeeded = false, ErrorMessage = "Urun bulunamadi" };

                    }
                }
                else
                {
                    return new ApplicationResponse { Succeeded = false, ErrorMessage = "Silmek istediginiz urun size ait degil" };

                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { ErrorMessage = ex.Message, Succeeded = false };
            }
        }

        public async Task<ApplicationResponse> Update(UpdateProductDto input, User applicationUser)
        {
            try
            {
                Product getExistProduct = await context.Products.FindAsync(input.ProductId);
                // urun sahibi ile aktif kullanicinin eslestigi kontrol edilir
                if (getExistProduct.CreatedBy == applicationUser.UserName)
                {
                    if (getExistProduct == null)
                    {
                        return new ApplicationResponse { Succeeded = false, ErrorMessage = "Urun bulunamadi" };
                    }

                    getExistProduct.ProductName = input.ProductName;
                    getExistProduct.Explanation = input.Explanation;
                    getExistProduct.CategoryId = GetCategoryId(input.Category);
                    getExistProduct.ColorId = GetColorId(input.Color);
                    getExistProduct.BrandId = GetBrandId(input.Brand);
                    getExistProduct.UsageStatusId = GetUsageStatusId(input.UsageStatus);
                    getExistProduct.ModifiedBy = applicationUser.UserName;
                    getExistProduct.ModifiedById = applicationUser.Id;
                    getExistProduct.ModifiedDate = DateTime.UtcNow;

                    context.Update(getExistProduct);
                    await context.SaveChangesAsync();

                    return new ApplicationResponse { Succeeded = true };
                }
                else
                {
                    return new ApplicationResponse { Succeeded = false,
                        ErrorMessage = "Guncellemek istediniz urun size ait degil" };
                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> AddImage(byte[] image, int productId, User applicationUser)
        {
            try
            {
                Product getExistProduct = await context.Products.FindAsync(productId);
                // urun sahibi ile aktif kullanicinin eslestigi kontrol edilir
                if (getExistProduct.CreatedBy == applicationUser.UserName)
                {
                    if (getExistProduct == null)
                    {
                        return new ApplicationResponse { Succeeded = false, ErrorMessage = "Urun bulunamadi" };
                    }

                    // kullanici profil fotografi olusturulur.
                    Photo newImage = new Photo();
                    newImage.CreatedBy = applicationUser.UserName;
                    newImage.CreatedById = applicationUser.Id;
                    newImage.CreatedDate = DateTime.UtcNow;
                    newImage.ProductId = productId;
                    newImage.Image = image;
                    await context.Photos.AddAsync(newImage);
                    await context.SaveChangesAsync();

                    // olusturulan urun resminin id si urun tablosuna guncellenir. hasPhoto degeri true olarak guncellenir.
                    Photo userPhoto = await context.Photos.Where(x => x.ProductId == productId).FirstAsync();
                    getExistProduct.PhotoId = userPhoto.Id;
                    getExistProduct.hasPhoto = true;
                    getExistProduct.ModifiedBy = applicationUser.UserName;
                    getExistProduct.ModifiedById = applicationUser.Id;
                    getExistProduct.ModifiedDate = DateTime.UtcNow;

                    context.Update(getExistProduct);

                    await context.SaveChangesAsync();

                    return new ApplicationResponse { Succeeded = true };
                }
                else
                {
                    return new ApplicationResponse { Succeeded = false,
                        ErrorMessage = "Guncellemek istediginiz urun size ait degil" };
                }
                    
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> DeleteImage(int productId, User applicationUser)
        {
            try
            {
                Product getExistProduct = await context.Products.FindAsync(productId);
                // urun sahibi ile aktif kullanicinin eslestigi kontrol edilir
                if (getExistProduct.CreatedBy == applicationUser.UserName)
                {
                    if (getExistProduct == null)
                    {
                        return new ApplicationResponse { Succeeded = false, ErrorMessage = "Urun bulunamadi" };
                    }

                    var imageId = context.Products.Where(x => x.ProductId == productId).FirstOrDefault().PhotoId;
                    // silinecek olan kullanici profil fotografi database den alinir.
                    Photo willDeleteImage = context.Photos.Where(x => x.Id == imageId).FirstOrDefault();
                    if (willDeleteImage == null)
                    {
                        return new ApplicationResponse { Succeeded = false, ErrorMessage = "Urun resmi bulunamadi" };
                    }
                    context.Photos.Remove(willDeleteImage);
                    await context.SaveChangesAsync();

                    // silinen urun resminin product tablosundaki karsiligi guncellenir.
                    getExistProduct.PhotoId = 0;
                    getExistProduct.hasPhoto = false;
                    getExistProduct.ModifiedBy = applicationUser.UserName;
                    getExistProduct.ModifiedById = applicationUser.Id;
                    getExistProduct.ModifiedDate = DateTime.UtcNow;

                    context.Update(getExistProduct);
                    await context.SaveChangesAsync();

                    return new ApplicationResponse { Succeeded = true };
                }
                else
                {
                    return new ApplicationResponse
                    {
                        Succeeded = false,
                        ErrorMessage = "Guncellemek istediginiz urun size ait degil"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ApplicationResponse> OpenToOffer(int productId, User applicationUser)
        {
            try
            {
                Product getExistProduct = await context.Products.FindAsync(productId);
                // urun sahibi ile aktif kullanicinin eslestigi kontrol edilir
                if (getExistProduct.CreatedBy == applicationUser.UserName)
                {
                    if (getExistProduct == null)
                    {
                        return new ApplicationResponse { Succeeded = false, ErrorMessage = "Urun bulunamadi" };
                    }

                    getExistProduct.isOfferable = true;
                    getExistProduct.ModifiedBy = applicationUser.UserName;
                    getExistProduct.ModifiedById = applicationUser.Id;
                    getExistProduct.ModifiedDate = DateTime.UtcNow;

                    context.Update(getExistProduct);

                    await context.SaveChangesAsync();

                    return new ApplicationResponse { Succeeded = true };
                }
                else
                {
                    return new ApplicationResponse
                    {
                        Succeeded = false,
                        ErrorMessage = "Guncellemek istediginiz urun size ait degil"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApplicationResponse { Succeeded = false, ErrorMessage = ex.Message };
            }
        }


        //from memory cache
        string cacheKey = "default";

        private string GetCategoryName(int CategoryId)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<Category> casheList))
            {
                List<Category> categoryList = new List<Category>();
                var categories = context.Categories.ToListAsync().Result;
                foreach (var category in categories)
                {
                    categoryList.Add(category);
                }

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.Normal
                };
                memoryCache.Set(cacheKey, categoryList, cacheExpOptions);
                return categoryList.Where(x => x.CategoryId == CategoryId).FirstOrDefault().CategoryName;
            }
            return casheList.Where(x => x.CategoryId == CategoryId).FirstOrDefault().CategoryName;
        }

        //from memory cache
        private string GetColorName(int ColorId)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<Color> casheList))
            {
                List<Color> colorList = new List<Color>();
                var colors = context.Colors.ToListAsync().Result;
                foreach (var color in colors)
                {
                    colorList.Add(color);
                }

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.Normal
                };
                memoryCache.Set(cacheKey, colorList, cacheExpOptions);
                return colorList.Where(x => x.ColorId == ColorId).FirstOrDefault().ColorName;
            }
            return casheList.Where(x => x.ColorId == ColorId).FirstOrDefault().ColorName;
        }

        //from memory cache
        private string GetBrandName(int BrandId)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<Brand> casheList))
            {
                List<Brand> brandList = new List<Brand>();
                var brands = context.Brands.ToListAsync().Result;
                foreach (var brand in brands)
                {
                    brandList.Add(brand);
                }

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.Normal
                };
                memoryCache.Set(cacheKey, brandList, cacheExpOptions);
                return brandList.Where(x => x.BrandId == BrandId).FirstOrDefault().BrandName;
            }
            return casheList.Where(x => x.BrandId == BrandId).FirstOrDefault().BrandName;
        }

        //from memory cache
        private string GetUsageStatus(int UsageStatusId)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<UsageStatus> casheList))
            {
                List<UsageStatus> brandList = new List<UsageStatus>();
                var UsageStatuses = context.UsageStatus.ToListAsync().Result;
                foreach (var usageStatus in UsageStatuses)
                {
                    brandList.Add(usageStatus);
                }

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.Normal
                };
                memoryCache.Set(cacheKey, brandList, cacheExpOptions);
                return brandList.Where(x => x.UsageStatusId == UsageStatusId).FirstOrDefault().UsageStatusName;
            }
            return casheList.Where(x => x.UsageStatusId == UsageStatusId).FirstOrDefault().UsageStatusName;
        }

        // from memory cache
        private int GetCategoryId(string CategoryName)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<Category> casheList))
            {
                List<Category> categoryList = new List<Category>();
                var categories = context.Categories.ToListAsync().Result;
                foreach (var category in categories)
                {
                    categoryList.Add(category);
                }

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.Normal
                };
                memoryCache.Set(cacheKey, categoryList, cacheExpOptions);
                return categoryList.Where(x => x.CategoryName == CategoryName).FirstOrDefault().CategoryId;
            }
            return casheList.Where(x => x.CategoryName == CategoryName).FirstOrDefault().CategoryId;
        }

        // from memory cache
        private int GetColorId(string ColorName)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<Color> casheList))
            {
                List<Color> colorList = new List<Color>();
                var colors = context.Colors.ToListAsync().Result;
                foreach (var color in colors)
                {
                    colorList.Add(color);
                }

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.Normal
                };
                memoryCache.Set(cacheKey, colorList, cacheExpOptions);
                return colorList.Where(x => x.ColorName == ColorName).FirstOrDefault().ColorId;
            }
            return casheList.Where(x => x.ColorName == ColorName).FirstOrDefault().ColorId;
        }

        // from memory cache
        private int GetBrandId(string BrandName)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<Brand> casheList))
            {
                List<Brand> brandList = new List<Brand>();
                var brands = context.Brands.ToListAsync().Result;
                foreach (var brand in brands)
                {
                    brandList.Add(brand);
                }

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.Normal
                };
                memoryCache.Set(cacheKey, brandList, cacheExpOptions);
                return brandList.Where(x => x.BrandName == BrandName).FirstOrDefault().BrandId;
            }
            return casheList.Where(x => x.BrandName == BrandName).FirstOrDefault().BrandId;
        }

        // from memory cache
        private int GetUsageStatusId(string UsageStatusName)
        {
            if (!memoryCache.TryGetValue(cacheKey, out List<UsageStatus> casheList))
            {
                List<UsageStatus> brandList = new List<UsageStatus>();
                var UsageStatuses = context.UsageStatus.ToListAsync().Result;
                foreach (var usageStatus in UsageStatuses)
                {
                    brandList.Add(usageStatus);
                }

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.Normal
                };
                memoryCache.Set(cacheKey, brandList, cacheExpOptions);
                return brandList.Where(x => x.UsageStatusName == UsageStatusName).FirstOrDefault().UsageStatusId;
            }
            return casheList.Where(x => x.UsageStatusName == UsageStatusName).FirstOrDefault().UsageStatusId;
        }

        // from database
        private byte[] GetImage(Product product)
        {
            if(product.hasPhoto == true)
            {
                return context.Photos.Where(x => x.Id == product.PhotoId).FirstOrDefaultAsync().Result.Image;
            }
            else
            {
                return new byte[1];
            }
            
        }

        // frontend deki dropdown actionlari icin, sonrasinda frontend uzerinde yapilirsa kullanilabilir.
        public async Task<ApplicationResponse<List<Color>>> GetColors()
        {
            try
            {
                List<Color> list = await context.Colors.ToListAsync();

                return new ApplicationResponse<List<Color>>
                {
                    Succeeded = true,
                    Result = list
                };
            }
            catch (Exception e)
            {
                return new ApplicationResponse<List<Color>> { ErrorMessage = e.Message, Succeeded = false };
            }
        }
        public async Task<ApplicationResponse<List<Brand>>> GetBrands()
        {
            try
            {
                List<Brand> list = await context.Brands.ToListAsync();

                return new ApplicationResponse<List<Brand>>
                {
                    Succeeded = true,
                    Result = list
                };
            }
            catch (Exception e)
            {
                return new ApplicationResponse<List<Brand>> { ErrorMessage = e.Message, Succeeded = false };
            }
        }
        public async Task<ApplicationResponse<List<UsageStatus>>> GetUsageStatus()
        {
            try
            {
                List<UsageStatus> list = await context.UsageStatus.ToListAsync();

                return new ApplicationResponse<List<UsageStatus>>
                {
                    Succeeded = true,
                    Result = list
                };
            }
            catch (Exception e)
            {
                return new ApplicationResponse<List<UsageStatus>> { ErrorMessage = e.Message, Succeeded = false };
            }
        }
        
    }
}
