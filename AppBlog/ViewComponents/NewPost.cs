using AppBlog.Data.Abstact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppBlog.ViewComponents
{
public class NewPost:ViewComponent
{
      private IPostRepository _newpost;

        public NewPost(IPostRepository newpost)
        {
            _newpost = newpost;
        }

       public async Task<IViewComponentResult> InvokeAsync()
         {
            return View( await _newpost
                .Posts
                .OrderByDescending(p=>p.PublishedOn) //herbir post un azalan değere göre tarihini alacagız(publichheadon bizim tarihe verdiğimiz isim)
                .Take(5)
                .ToListAsync()
                );
         }
}
}