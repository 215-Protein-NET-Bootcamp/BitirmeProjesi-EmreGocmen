using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BuyWithOffer
{
    public class FileUpload
    {
        //default olarak png,jpg,jpeg ve gif kabul ediyor, extension hata verdigi icin default kullaniyorum.
        //[FileExtensions(Extensions = "png,jpg,jpeg")]
        public IFormFile image { get; set; }
    }
}
