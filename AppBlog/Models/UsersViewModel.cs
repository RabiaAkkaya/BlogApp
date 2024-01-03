
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppBlog.Models
{
    public class UsersViewModel
    {
   
    public int UserID { get; set; } 
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? UserName { get; set; }

    [EmailAddress]
    [Required]
    public string? Mail { get; set; }
   
    }
}




