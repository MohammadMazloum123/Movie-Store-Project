using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MovieMVC.Models.DTO;
using MovieMVC.Repositories.Abstract;

namespace MovieMVC.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private IUserAuthenticationService authService;
        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            this.authService = authService;
        }
        //public async Task<IActionResult> Register()
        //{
        //    var model = new RegisterationModel
        //    {
        //        Email = "admin@gmail.com",
        //        Username = "admin",
        //        Name = "Ravindra",
        //        Password = "Admin@123",
        //        PasswordConfirm = "Admin@123",
        //        Role = "Admin"
        //    };
        //    // if u want to register with user , change Role="User"
        //    var result = await authService.RegisterAsync(model);
        //    return Ok(result.Message);
            
        //}

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await authService.LoginAsync(model);
            if(result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = "Can't Log in ...";
                return RedirectToAction(nameof(Login));
            }
        }
        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();
                return RedirectToAction(nameof(Login));   
        }
    }
}
