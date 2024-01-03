using AppBlog.Entity;
using Microsoft.EntityFrameworkCore;

namespace AppBlog.Data.Concrete.EfCore
{
    public class BlogContext:DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options): base(options)
        {
//dışardan bir options bilgisi alabilmek (connection string) icin constructor oluşturduk. program cs de bu bilgiyi kullanacağız
        }
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<User> Users => Set<User>();

    }
}