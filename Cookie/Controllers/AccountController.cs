using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cookie.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Cookie.Controllers
{
    public class AccountController : Controller
    {
        public List < UserModel > users = null;  
        public AccountController() {  
            users = new List < UserModel > ();  
            users.Add(new UserModel() {  
                UserId = 1, Username = "Anoop", Password = "123",Role = "User"
            });  
            users.Add(new UserModel() {  
                UserId = 2, Username = "Other", Password = "123",Role = "Admin"
            });  
        }  
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = users.Where(x => x.Username == loginModel.UserName && x.Password == loginModel.Password).FirstOrDefault();
                if (user ==null)
                {
                    ViewBag.Message = "账号密码错误";  
                    return View(loginModel);  
                }
                else
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        //new Claim(ClaimTypes.Role,user.Role)
                    };

                    var role = users.Where(x => x.Role == user.Role);
                    foreach (var item in role)
                    {
                        claims.Add(new Claim(ClaimTypes.Role,item.Role));
                    }
                    
                    var Identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(Identity));
                    return LocalRedirect("/");
                }
            }
            return View(loginModel);
            {
                
            }
        }
        public async Task < IActionResult > LogOut() {  
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  
            //Redirect to home page    
            return LocalRedirect("/");  
        }  
        
        
        [HttpPost]
        public async Task<IActionResult> JWTLogin(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = users.Where(x => x.Username == loginModel.UserName && x.Password == loginModel.Password).FirstOrDefault();
                if (user ==null)
                {
                    ViewBag.Message = "账号密码错误";  
                    return View(loginModel);  
                }
                else
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        //new Claim(ClaimTypes.Role,user.Role)
                    };

                    var role = users.Where(x => x.Role == user.Role);
                    foreach (var item in role)
                    {
                        claims.Add(new Claim(ClaimTypes.Role,item.Role));
                    }
                    
                    return LocalRedirect("/Account/Cookie");
                }
            }
            return View(loginModel);
            {
                
            }
        }

    }

    public class UserModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
    }
}