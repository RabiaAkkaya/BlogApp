
using AppBlog.Data.Abstact;
using AppBlog.Data.Concrete.EfCore;
using AppBlog.Entity;


namespace AppBlog.Data.Concrete
{
    public class EfUserRepository : IUserRepository
    {
        
        private readonly BlogContext _context;
        public EfUserRepository(BlogContext context)
        {
            _context=context;
        }

        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User User)
        {
          _context.Users.Add(User);
          _context.SaveChanges();
        }

        public User GetUserByID(int id)
        {
        return _context.Users.Find(id);
        }

        public void UpdateUser(User user)
        {
            var user2=_context.Users.FirstOrDefault(x=>x.UserID==user.UserID);
            if(user2 !=null)
            {
                if(user.Image !=null)
                {
                     user2.Image=user.Image;
                }
            user.UserID=user.UserID;
            user2.Name=user.Name;
            user2.UserName=user.UserName;        
            user2.Mail=user.Mail;
           
            _context.SaveChanges();
            }
           

        }
    }
}