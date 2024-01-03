using System.ComponentModel.DataAnnotations;

namespace AppBlog.Models
{
    public class RegisterViewModel
    {
   
        [Required]
        [EmailAddress]
        [Display(Name="Eposta")]
        public string? Mail { get; set; }
        
        [Required]
        [Display(Name="Username")]
         public string? UserName { get; set; }
        [Required]
        [Display(Name="Ad Soyad")]
         public string? Name { get; set; }

        [Required]
        [StringLength(10, ErrorMessage ="{0} alanı en az {2} karakter uzunluğunda olmalıdır.",MinimumLength=2)]
        [DataType(DataType.Password)]
        [Display(Name ="Parola")]
        public string? Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Parola Tekrar")]
        [Compare(nameof(Password),ErrorMessage ="Parolanız eşleşmiyor!")]//karşılaştır
        public string? ConfirmPassword { get; set; }

    }
}