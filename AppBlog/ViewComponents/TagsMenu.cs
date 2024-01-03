using AppBlog.Data;
using AppBlog.Data.Abstact;
using AppBlog.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppBlog.ViewComponents
{
    public class TagsMenu:ViewComponent
    {
        private ITagRepository _tagsmenu;
        public TagsMenu(ITagRepository tagsmenu)
        {
            _tagsmenu=tagsmenu;
            
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _tagsmenu.Tags.ToListAsync());

        }
       
    }

}