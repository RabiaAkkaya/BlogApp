using System.ComponentModel.DataAnnotations;
using AppBlog.Entity;

namespace AppBlog.Models
{
    public class EditPostViewModel
    {
        public int PostID { get; set; }
        
        [Required]    
        [Display(Name="Başlık")]
        public string? Title { get; set; }

        [Required]      
        [Display(Name ="İçerik")]
        public string? Content { get; set; }

        [Required]      
        [Display(Name ="Açıklama")]
        public string? Description { get; set; }

        [Required]      
        [Display(Name ="Url")]
        public string? Url { get; set; }

        [Required]
        public bool IsActive { get; set; }

       
    }
}