using AppBlog.Entity;

namespace AppBlog.Data
{
   public interface ITagRepository
   {
   IQueryable<Tag> Tags{get;}
   void CreateTag(Tag tag);
   }

}