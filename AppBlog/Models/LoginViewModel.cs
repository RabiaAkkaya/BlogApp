using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppBlog.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name="Eposta")]
        public string? Mail { get; set; }

        [Required]
        [StringLength(10, ErrorMessage ="{0} alanı en az {2} karakter uzunluğunda olmalıdır.",MinimumLength=2)]
        [DataType(DataType.Password)]
        [Display(Name ="Parola")]
        public string? Password { get; set; }
    }
}