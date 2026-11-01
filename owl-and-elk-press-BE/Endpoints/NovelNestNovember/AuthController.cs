using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace owl_and_elk_press_BE.Endpoints.NovelNestNovember;

    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet("google")]
        public IActionResult SignInWithGoogle()
        {
            var properties = new AuthenticationProperties 
            { 
                RedirectUri = Url.Action("GoogleCallback") 
            };
            
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            
            if (!result.Succeeded)
            {
                return BadRequest("Google authentication failed.");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            // TODO: Qui puoi salvare l'utente nel database se non esiste e generare il tuo JWT token

            return Redirect("https://owlandelkpress.com"); 
        }
    }
