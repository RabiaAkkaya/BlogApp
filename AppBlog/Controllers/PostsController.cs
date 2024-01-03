using System.Net;
using System.Security.Claims;
using AppBlog.Data;
using AppBlog.Data.Abstact;
using AppBlog.Data.Concrete.EfCore;
using AppBlog.Entity;
using AppBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppBlog.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postrepository;
        private ITagRepository _tagrepository;
        private ICommentRepository _commentrepository;
      
    

        public PostsController(IPostRepository postrepository,ITagRepository tagrepository, ICommentRepository commentrepository)
        {
            _postrepository=postrepository;
            _tagrepository=tagrepository;
            _commentrepository=commentrepository;
        }

        public async Task<IActionResult> Index(string tag,int? pageNumber)
        { 
            int pageSize = 5;
            var posts=_postrepository.Posts.Where(i=>i.IsActive==true);
            if (pageNumber.HasValue && pageNumber > 0)
    {
        posts = posts
            .Skip((pageNumber.Value - 1) * pageSize)
            .Take(pageSize);
    }

               
            var claims=User.Claims;
           
            if(!string.IsNullOrEmpty(tag))
            {
                posts=posts.Where(x=>x.Tags.Any(t=>t.url==tag));
            }

            return View( 
                new PostsViewModel
                {
                    Posts= await posts.ToListAsync()
                });
                
            
        }
        public async Task<IActionResult> Details(string url)
        {
           return View(await _postrepository
             .Posts
             .Include(x=>x.User)
             .Include(x=>x.Tags)
             .Include(x=>x.Comments)
             .ThenInclude(x=>x.User)//theninclude ile her commentin user bilgisini alabiliyoruz
             .FirstOrDefaultAsync(p=>p.url==url));
        }
        [HttpPost]
        public JsonResult AddComment(int PostID,string Text,int CommentID)
        {
            var userID=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName=User.FindFirstValue(ClaimTypes.Name);
            var avatar=User.FindFirstValue(ClaimTypes.UserData);

            var entity=new Comment
                { 
                    CommentID=CommentID,
                    Text=Text,
                     PublishedOn=DateTime.Now,
                     PostID=PostID,
                     UserId=int.Parse(userID ?? "")//nulda boş string bilgi döndür
                    

                };
            
        _commentrepository.AddComment(entity);
            return Json( new
            {
              userName,Text,entity.PublishedOn,CommentID,avatar
            }
            );
        }
        [HttpPost]
        [Authorize]
        public JsonResult RemoveComment(int CommentID)
        {   var comment=_commentrepository.GetCommentByID(CommentID);    
        
          _commentrepository.DeleteComment(comment);
            return Json(new {success=true});
      
        }
    

        [HttpPost]       
        public JsonResult RemovePost(int PostID)
        {   
          var post=_postrepository.GetPostById(PostID);   
      
          if (post != null)
    {
        _postrepository.DeletePost(post);
        return Json(new { success = true });
    }
    else
    {
        // Silinecek bir post bulunamadıysa hata mesajı döndür
        return Json(new { success = false, error = "Post not found" });
    }
      
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
            var userID=User.FindFirstValue(ClaimTypes.NameIdentifier);
            _postrepository.CreatePost(new Post
            {
                Title=model.Title,
                Content=model.Content,
                Description=model.Description,
                url=model.Url,
                UserId=int.Parse(userID ??""),
                PublishedOn=DateTime.Now,
                Image="1.png",
                IsActive=false

            });
            return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> List()
        {
            var userID=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role=User.FindFirstValue(ClaimTypes.Role);
             var posts=_postrepository.Posts;
            if(string.IsNullOrEmpty(role))//rol boşşa
            {
            posts=posts.Where(x=>x.UserId==userID);
             
            }
           return View(await posts.ToListAsync());
           
        }
        [Authorize]
        public IActionResult Edit(int id)
     {
    // Eğer belirtilen ID'ye sahip bir gönderi yoksa NotFound döndür
    var post = _postrepository.Posts.FirstOrDefault(x => x.PostID == id);
  
    var role=User.FindFirstValue(ClaimTypes.Role);
    if(ModelState.IsValid)
    {
      if (post != null)
        {
    
        var viewModel = new EditPostViewModel
    {
        PostID = post.PostID,
        Title = post.Title,
        Url=post.url,
        Description = post.Description,
        Content = post.Content,
        IsActive = post.IsActive 
    
    };
     return View(viewModel);
        }
  
   else{
     return NotFound();
   }
    }
  return View();

    
}
        [Authorize]
        [HttpPost]
         public IActionResult Edit(EditPostViewModel model)
        {
            if(ModelState.IsValid)
            {
          var entity=new Post{
            PostID=model.PostID,
            Title=model.Title,
            Description=model.Description,
            Content=model.Content,
            IsActive=model.IsActive,
            url=model.Url
          }; 
          _postrepository.UpdatePost(entity);
             return RedirectToAction("List");  
         
           
           
            }       
            
             return View(model);
            
        }
        public IActionResult UpdatePostStatus()
        {
            return View("List");
        }
      

[HttpPost]
public JsonResult UpdatePostStatus(int PostID)
{
    try
    {
        var post = _postrepository.GetPostById(PostID);

        if (post != null)
        {
            post.IsActive = !post.IsActive; // Aktif durumu tersine çevir
            _postrepository.UpdateActivePost(post);

            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false });
        }
    }
    catch (Exception ex)
    {
        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        Console.Error.WriteLine($"Durum güncelleme işlemi başarısız. Hata: {ex.Message}");
        return Json(new { success = false, error = "Durum güncelleme işlemi başarısız. Hata: " + ex.Message });
    }
}



    }
}