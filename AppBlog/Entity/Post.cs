namespace AppBlog.Entity;

public class Post
{
public int PostID { get; set; }
public string? Title{ get; set; }
public string? Content{ get; set; }
public string? Description{ get; set; }
public DateTime PublishedOn { get; set; } 
public bool IsActive{ get; set; }
public string? Image { get; set; }
public int UserId {get; set;}
public string? url {get; set;}
public User User { get; set; }=null!;
public List<Tag> Tags{ get; set; }=new List<Tag>();
public List<Comment> Comments{ get; set; }=new List<Comment>();


}
