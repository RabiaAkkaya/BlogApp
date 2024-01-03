using System.Data.Common;
using AppBlog.Data.Abstact;
using AppBlog.Data.Concrete.EfCore;
using AppBlog.Entity;

namespace AppBlog.Data.Concrete
{
    public class EfCommentRepository : ICommentRepository
    {
        private BlogContext _context;
       public EfCommentRepository(BlogContext context)
       {
          _context=context;
       }

        public IQueryable<Comment> Comments => _context.Comments;

        public void AddComment(Comment comment)
        {
           _context.Add(comment);
           _context.SaveChanges();
        }
        

        public Comment GetCommentByID(int commentID)
        {
            return _context.Comments.Find(commentID);
        }
        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }
    }

}