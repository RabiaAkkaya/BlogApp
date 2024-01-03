using AppBlog.Data.Concrete.EfCore;
using AppBlog.Entity;

namespace AppBlog.Data.Concrete
   {
    public class EfTagRepository : ITagRepository
    {
        BlogContext _context;

        public EfTagRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> Tags =>_context.Tags;

        public void CreateTag(Tag tag)
        {
          _context.Add(tag);
          _context.SaveChanges();
        }
    }

} 
