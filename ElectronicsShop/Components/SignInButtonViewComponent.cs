using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ElectronicsShop.Components
{
    public class SignInButtonViewComponent :ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated == true) { return View("UserSigned", User.Identity.Name); }
                return View(new LoginModel ());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IViewComponentResult> Invoke(LoginModel loginModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        IdentityUser user = await userManager.FindByNameAsync(loginModel.Name);
        //        if (user != null)
        //        {
        //            await signInManager.SignOutAsync();
        //            //if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
        //            //{
        //            //    return Redirect("/");
        //            //}
        //        }
        //    }
        //    ModelState.AddModelError("", "Invalid name or password");
        //    return View(loginModel);
        //}

        //private IActionResult Redirect(string v)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IActionResult> Logout(string returnUrl = "/")
        //{
        //    await signInManager.SignOutAsync();
        //    return Redirect(returnUrl);
        //}
    }
}
