using MainApi.Logger;
using MainApp.AuthService;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MainApp.Controllers
{
    public class AccauntController : Controller
    {
        private readonly AuthServices _authService;


        public AccauntController(AuthServices authServices)
        {
            _authService = authServices;   
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = await _authService.LoginAsync(email, password);

            if (token != null)
            {
                Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return View(model);
                }

                var result = await _authService.RegisterAsync(model.Email, model.Password, model.ConfirmPassword);
                if (result)
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                LoggerConfig.LogInformation(ex.Message);
            }
            finally
            {
                LoggerConfig.CloseLogger();
            }
            

            ModelState.AddModelError(string.Empty, "Error during registration.");
            return View(model);
        }
    }
}
