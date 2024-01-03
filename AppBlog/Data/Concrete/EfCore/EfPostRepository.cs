using AppBlog.Data.Abstact;
using AppBlog.Data.Concrete.EfCore;
using AppBlog.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppBlog.Data.Concrete
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context=context;
        }
        public IQueryable<Post> Posts => _context.Posts;      

     
        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void DeletePost(Post post)
        {
           _context.Posts.Remove(post);
           _context.SaveChanges();
        }
        public Post GetPostById(int id)
        {
            return _context.Posts.Find(id);
        }
      

        public void UpdatePost(Post post)
        {
            var entity=_context.Posts.FirstOrDefault(i=>i.PostID==post.PostID);

            if(entity !=null)
            {
            entity.PostID=post.PostID;
            entity.Title=post.Title;
            entity.Description=post.Description;
            entity.Content=post.Content;
            entity.IsActive=post.IsActive;
            entity.url=post.url;

        
           _context.SaveChanges();
            }
        }
          public void UpdateActivePost(Post post)
        {
            var entity=_context.Posts.FirstOrDefault(i=>i.PostID==post.PostID);

            if(entity !=null)
            {
            entity.IsActive=post.IsActive;  
           _context.SaveChanges();
            }
        }
    }
}