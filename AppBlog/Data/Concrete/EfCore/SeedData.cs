using AppBlog.Entity;
using Microsoft.EntityFrameworkCore;

namespace AppBlog.Data.Concrete.EfCore
{
public static class SeedData
{
    public static void TestVerileriniDoldur(IApplicationBuilder app)
    {
        var context=app.ApplicationServices.CreateAsyncScope().ServiceProvider.GetService<BlogContext>();
        if(context != null)
        {
            if(context.Database.GetPendingMigrations().Any())//uygulanmamış migration varsa
            {
                 context.Database.Migrate();//database update karşılığıdır.
            }

            if(!context.Tags.Any())//ilgili tabloda herhangi bir kayıt yoksa
            {
                context.Tags.AddRange(
                 new Tag{Text="web programlama", url="web-programlama",Color=TagColors.warning},
                 new Tag{Text="backend",url="bakend-programlama",Color=TagColors.success},
                 new Tag{Text="frontend",url="frontend-programlama",Color=TagColors.secondary},
                 new Tag{Text="fullstack",url="full-stack-programlama",Color=TagColors.primary}

                );

                context.SaveChanges();

            }
            if(!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { UserName="rabik",Name="RabiaAkkaya", Image="1.png", Mail="rabia@gmail.com",Password="123"},
                    new User { UserName="samos",Name="SametAkkaya",Image="2.png",Mail="samet@gmail.com",Password="123"}

                );
                context.SaveChanges();
            }
            if(!context.Posts.Any())
            {
                context.Posts.AddRange(
                   new Post {
                    Title="Asp.net Core",
                    Content="Asp.net core dersleri",
                    Description="Asp.net core dersleri",
                    IsActive=true,
                    url="aspnet-core",
                    PublishedOn= DateTime.Now.AddDays(-10),
                    Tags=context.Tags.Take(3).ToList(),
                    UserId=1,
                    Image="1.png"

                   },
                   new Post {
                    Title="Asp.net Core",
                    Content="Asp.net core dersleri",
                    Description="Asp.net core dersleri",
                    IsActive=true,
                     url="aspnet-core-ders",
                    PublishedOn= DateTime.Now.AddDays(-20),
                    Tags=context.Tags.Take(2).ToList(),
                    UserId=1,
                    Image="2.png",
                    Comments=new List<Comment>{
                        new Comment{ Text="iyi bir kurs",PublishedOn=new DateTime(), UserId=1},
                        new Comment{ Text="çok faydalandığım bir kurs",PublishedOn=new DateTime(), UserId=2}}

                   },
                   new Post {
                    Title="Asp.net Core",
                    Content="Asp.net core dersleri",
                    Description="Asp.net core dersleri",
                    IsActive=true,
                    url="aspnet-core-ders2",
                    PublishedOn= DateTime.Now.AddDays(-5),
                    Tags=context.Tags.Take(4).ToList(),
                    UserId=2,
                    Image="2.png"

                   }
                );
                context.SaveChanges();
            }
        }

    }
}
}