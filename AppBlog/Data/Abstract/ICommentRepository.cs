using AppBlog.Entity;

namespace AppBlog.Data.Abstact
{
    public interface ICommentRepository
    {
      Comment GetCommentByID(int commentID);
      public void AddComment(Comment comment);
      public void DeleteComment(Comment comment);
      public IQueryable<Comment> Comments{get;}
     
    }
}