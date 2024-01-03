using AppBlog.Entity;

namespace AppBlog.Data.Abstact
{
    public interface IPostRepository
    {
        public IQueryable<Post> Posts {get;}
        public void CreatePost(Post post);
        public void UpdatePost(Post post);
        public void DeletePost(Post post);
         public Post GetPostById(int id);
         public void UpdateActivePost(Post post);
    }
}