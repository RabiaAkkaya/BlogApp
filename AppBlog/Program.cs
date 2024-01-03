using AppBlog.Data;
using AppBlog.Data.Abstact;
using AppBlog.Data.Concrete;
using AppBlog.Data.Concrete.EfCore;
using AppBlog.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder=WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogContext>(options=>
{
     var config=builder.Configuration;
     var connectionString=config.GetConnectionString("mysql_connection");
     var version= new MySqlServerVersion(new Version(5,2,1));
     
     options.UseMySql(connectionString,version);
});

builder.Services.AddScoped<IPostRepository,EfPostRepository>();
builder.Services.AddScoped<ITagRepository,EfTagRepository>();
builder.Services.AddScoped<ICommentRepository,EfCommentRepository>();
builder.Services.AddScoped<IUserRepository,EfUserRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>{
   options.LoginPath="/Users/Login";
});//uygulamanın bir cookie ile kullanıcı tanıma özelliğini kullanacağız

 var app=builder.Build();
  app.UseStaticFiles();

 app.UseRouting();//aşağıdaki iki middlewire den önce mutlaka routing in yapılandırılması gerekir.

 app.UseAuthentication();//uygulamanın bizi tanımasını sağlıcaz
 app.UseAuthorization();//yetkilendirme

 SeedData.TestVerileriniDoldur(app);

 app.MapControllerRoute(
    name: "post_details",
    pattern:"posts/details/{url}",
    defaults: new {controller="Posts", action="Details"}

 );
 app.MapControllerRoute(
    name: "post_page",
    pattern:"posts/index/{pageNumber}",
    defaults: new {controller="Posts", action="Index"}

 );
  app.MapControllerRoute(
    name: "posts_by_tag",
    pattern:"posts/tag/{tag}",
    defaults: new {controller="Posts", action="Index"}

 );
   app.MapControllerRoute(
    name: "user_profile",
    pattern:"user/{username}",
    defaults: new {controller="Users", action="Profile"}

 );
 
 app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{Id?}"
 );
 app.Run();

