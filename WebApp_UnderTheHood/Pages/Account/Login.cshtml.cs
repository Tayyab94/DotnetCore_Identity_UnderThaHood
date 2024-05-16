using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credentials { get; set; } = new Credential();
        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();
            if (Credentials.UserName.Trim() == "admin" && Credentials.Password.Trim() == "admin") {

                var claims = new List<Claim>
                {

                    new Claim(ClaimTypes.Name,Credentials.UserName),
                    new Claim(ClaimTypes.Email,"admin@.com"),
                    new Claim("Department","HR"),
                    new Claim("admin","true"),
                    new Claim("Manager","true"),
                    new Claim("Empdate","2024-02-01")
                };

                //var identity = new ClaimsIdentity(claims,"MyCookieAuth");

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credentials.IsRemember,
                };

                //await HttpContext.SignInAsync("MyCookieAuth", claimPrincipal);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, authProperties);

                return Redirect("/index");
            }

            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name ="User Name")]
        public string UserName { get; set; }= string.Empty;
        [Required]
        public string Password { get; set; }= string.Empty;

        [Display(Name ="Remember Me")]
        public bool IsRemember {  get; set; }
    }
}
