using AppBlog.Entity;
using AppBlog.Models;

namespace AppBlog.Data.Abstact
{
    public interface IUserRepository
    {
        IQueryable<User>Users{get;}
        void CreateUser(User User);
        void UpdateUser(User user);
        User GetUserByID(int id);
    }

}