namespace AppBlog.Entity;

public class User
{
public int UserID { get; set; }
public string? UserName { get; set; }
public string? Name { get; set; }
public string? Mail{ get; set; }
public string? Password { get; set; }     
public string? Image { get; set; }
public List<Post> Posts {get; set;}= new List<Post>();
public List<Comment> Comments {get; set;}= new List<Comment>();

}
