using TestNewLine.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNewLine.Core.Dtos
{
    public class CreatePostBlogDto
    {

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "المؤلف")]
        public string AuthorId { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "العنوان")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "العنوان الفرعي")]
        public string? SubTittle { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "الصورة")]
        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "التفاصيل")]
        public string? Body { get; set; }




    }
}
