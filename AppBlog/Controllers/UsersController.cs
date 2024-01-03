

using System.Security.Claims;
using AppBlog.Data.Abstact;
using AppBlog.Entity;
using AppBlog.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBlog.Controllers
{

  public class UsersController : Controller
  {

    private readonly IUserRepository _userRepo;
    public UsersController(IUserRepository userRepo)
    {
      _userRepo = userRepo;
    }
    public IActionResult Login()
    {
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index", "Posts");
      }
      return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      if (ModelState.IsValid)
      {
        var isUSer = _userRepo.Users.FirstOrDefault(x => x.Mail == model.Mail && x.Password == model.Password);
        if (isUSer != null)
        {

          var userClaims = new List<Claim>();
          userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUSer.UserID.ToString()));
          userClaims.Add(new Claim(ClaimTypes.Name, isUSer.UserName ?? ""));
          userClaims.Add(new Claim(ClaimTypes.GivenName, isUSer.Name ?? ""));
          userClaims.Add(new Claim(ClaimTypes.GivenName, isUSer.Image ?? ""));

          if (isUSer.Mail == "rabia@gmail.com")
          {
            userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
          }
          var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
          var authProperties = new AuthenticationProperties
          {
            IsPersistent = true//beni hatırla check i için
          };
          await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);//önceden giriş işlemleri varsa sil

          await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
           new ClaimsPrincipal(claimsIdentity), authProperties);
          return RedirectToAction("Index", "Posts");


        }
        else
        {

          ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış!");
        }
      }

      return View(model);
    }
    public IActionResult Register()
    {
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Login", "Users");
      }
      return View();

    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {

      if (ModelState.IsValid)
      {
        var user = await _userRepo.Users.FirstOrDefaultAsync(x => x.Mail == model.Mail || x.UserName == model.UserName);
        if (user == null)
        {
          _userRepo.CreateUser(new User
          {
            Mail = model.Mail,
            Password = model.Password,
            UserName = model.UserName,
            Name = model.Name,
            Image = "profil.png"

          });
          return RedirectToAction("Login");
        }
        else
        {
          ModelState.AddModelError("", "Kullanıcı adı ya da email kullanımda!");
        }
      }

      return View(model);


    }
    public async Task<IActionResult> LogOut()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Login");
    }
    public IActionResult Profile(string username)
    {
      if (string.IsNullOrEmpty(username))
      {
        return NotFound();
      }
      var user = _userRepo
      .Users
      .Include(x => x.Posts)
      .Include(x => x.Comments)
      .ThenInclude(x => x.Post)
      .FirstOrDefault(x => x.UserName == username);
      if (user == null)
      {
        return NotFound();
      }
      return View(user);
    }

    [HttpGet]
    public IActionResult ProfileSettings2(int id)
    {
      id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
      if (!User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Login", "Users");
      }
      var user = _userRepo.GetUserByID(id);
      if (user != null)
      {
        var entity = new UsersViewModel2
        {
          UserID = user.UserID,
          Image = user.Image                 
        
        };
        
      
        return View(entity);
      }
      return RedirectToAction("Login");

    }
[HttpGet]
    public IActionResult ProfileSettings(int id)
    {
      id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
      if (!User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Login", "Users");
      }
      var user = _userRepo.GetUserByID(id);
      if (user != null)
      {
        var entity = new UsersViewModel
        {
          UserID = user.UserID,                         
          Name = user.Name,
          UserName = user.UserName,
          Mail = user.Mail

        };
        
      
        return View(entity);
      }
      return RedirectToAction("Login");

    }
    [HttpPost]
    public JsonResult ProfileSettings2(UsersViewModel2 model, IFormFile file)
    {
      if (!User.Identity.IsAuthenticated)
      {
        return Json(new { redirectToAction = Url.Action("Users", "Login") });

      }
      if (file != null && file.Length > 0)
      {

        // Yeni resmin adını oluştur
        string imageExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        if (imageExtension != ".png" && imageExtension != ".jpeg" && imageExtension != ".jpg")
        {
          var errorResponse = new { success = false, message = "Hata: Sadece Fotoğraf Yükleyiniz! " };
          return Json(errorResponse);
        }
        string ImageName = "your_fixed_image_name.jpeg"; // Sabit bir isim kullanılacak

        // Resim dosyasının tam yolu
        string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Images/{ImageName}");

        // Eğer dosya zaten varsa sil
        if (System.IO.File.Exists(path))
        {
          System.IO.File.Delete(path);
        }

        // Yeni resmi oluştur
        using var stream = new FileStream(path, FileMode.Create);

        // Resmin adını model içinde güncelle
        model.Image = ImageName;

        // Dosyayı kopyala
        file.CopyTo(stream);
      }

      var entity = new User
      {
        UserID = model.UserID,
        Image = model.Image      

      };

      Console.WriteLine(model.Image);
      _userRepo.UpdateUser(entity);
      return Json(new { success = true, user = entity });
    }
     [HttpPost]
    public JsonResult ProfileSettings(UsersViewModel model)
    {
       if (!ModelState.IsValid)
    {
        return Json(new { success = false, message = "Hatalı model" });
    }
    

      var entity = new User
      {
        UserID = model.UserID,      
        Name = model.Name,
        UserName = model.UserName,
        Mail = model.Mail,

      };

      _userRepo.UpdateUser(entity);
      return Json(new { success = true, user = entity });
    }



  }
}
