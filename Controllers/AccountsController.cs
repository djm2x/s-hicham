// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System.Text;
// using System.Security.Claims;
// using Models;
// using Services;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.WebUtilities;
// using System.Text.Encodings.Web;
// using Providers;
// using Microsoft.Extensions.Options;

// namespace Controllers
// {
//     [Route("api/[controller]/[action]")]
//     [ApiController]
//     public class AccountsController : SuperController<Utilisateur>
//     {
//         private  TokkenHandler _tokkenHandler;
//         private  EmailService _emailService;
//         private  AccountService _accountService;
//         private  HtmlService _htmlService;
//         private  EmailSettings _emailSettings;
//         public AccountsController(MyContext context
//         , EmailService emailService
//         , AccountService accountService
//         , TokkenHandler tokkenHandler
//         , HtmlService htmlService
//         , IOptions<EmailSettings> emailSettings
//         )
//         : base(context)
//         {
//             _emailService = emailService;
//             _accountService = accountService;
//             _tokkenHandler = tokkenHandler;
//             _htmlService = htmlService;
//             _emailSettings = emailSettings.Value;
//         }

//         [HttpPost]
//         public async Task<ActionResult<Utilisateur>> Create(Utilisateur model)
//         {
//             try
//             {
                
//                 _context.Utilisateurs.Add(model);
//                 await _context.SaveChangesAsync();

                
//             }
//             catch (DbUpdateConcurrencyException ex)
//             {
//                 return BadRequest(new { message = ex.Message });
//             }

//             return Ok(model);
//         }

        

//         [HttpGet("{email}/{returnUrl}/{lang}")]
//         public async Task<ActionResult> SendEmailForResetPassword(string email, string returnUrl, string lang)
//         {
//             var model = await _context.Utilisateurs.SingleOrDefaultAsync(e => e.Email == email);

//             if (model == null)
//             {
//                 return Ok(new { message = "email incorrect", code = -1 });
//             }

//             // var message = lang == "fr" ? $"<h3>Votre lien de reinitialisation</h3>" : $"<h3>Your reset link</h3>";

//             try
//             {
//                 var subject = lang == "fr" ? $"Code de réinitialisation de mot de pass" : $"Password reset code";
//                 var codeContainEmail = $"{model.Email}*{DateTime.Now}";
//                 var activationLink = _accountService.GenerateActivationLink(Request, returnUrl, codeContainEmail);
//                 string html = "";

//                 html = await this._htmlService.GenerateHtmlReset(
//                         HtmlEncoder.Default.Encode(activationLink)
//                         , _emailSettings.SenderEmail
//                         , $"{model.Email}"
//                         , lang
//                     );

//                 await _emailService.SendEmailAsync(model.Email, subject, html);
//             }
//             catch (DbUpdateConcurrencyException ex)
//             {
//                 return BadRequest(ex.Message);
//             }

//             return Ok(new { message = "Le lien de réinitialisation de mot de passe a été envoyé à votre boite email.", code = 1 });
//         }



//         [HttpPost]
//         public async Task<ActionResult> ResetPassword([FromBody] UtilisateurDTO model)
//         {
//             var user = await _context.Utilisateurs.SingleOrDefaultAsync(e => e.Email == model.Email);

//             if (user == null)
//             {
//                 return Ok(new { message = "Email Incorrect", code = -1 });
//             }

//             user.Password = model.Password;

//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(ex.Message);
//             }

//             return Ok(new { message = "Mot de passe réinitialisé avec succès, on attend votre connexion.", code = 1 });
//         }

//         [HttpPost]
//         public async Task<ActionResult<Utilisateur>> LogIn(UtilisateurDTO model)
//         {
//             if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
//                 return Ok(new { message = "Login ou Mot sont vide", code = -4 });

//             var user = await _context.Utilisateurs.SingleOrDefaultAsync(x => x.Email == model.Email);
//             // check if Nom exists
//             if (user == null)
//                 return Ok(new { message = "Login érroné, vous pouvez utiliser l'option de réinitialisation de mot de passe.", code = -3 });

//             if (user.Password == model.Password)
//             {
                

//                 // remove password before returning
//                 user.Password = "";

//                 // if (user.IsActive == false)
//                 // {
//                 //     return Ok(new { message = "Veuillez patienter que votre compte soit active par l'administration", code = -2 });
//                 // }

//                 var claims = new Claim[]
//                     {
//                         new Claim(ClaimTypes.Name, user.Id.ToString()),
//                         new Claim(ClaimTypes.Email, user.Email),
//                         new Claim("idRegion", user.IdTypeprofil==2? user.IdRegion.ToString() : "0"),
//                     };

//                 return Ok(new { code = 1, user, token = _tokkenHandler.GenerateTokken(claims), message = "Suivi des activités des services déconcentrés." });
//             }

//             return Ok(new { message = "Mot de passe érroné, vous pouvez utiliser l'option de réinitialisation de mot de passe.", code = -1 });
//         }
//     }

//     public class TokenDTO
//     {
//         public int IdUtilisateur { get; set; }
//         public int IdPlace { get; set; }
//         public int IdRulePlace { get; set; }
//         public string Email { get; set; }
//         public string UtilisateurProfil { get; set; }
//     }

//     public class UtilisateurDTO
//     {
//         public string Email { get; set; }
//         public string Password { get; set; }
//         public Profil Profil { get; set;}
//     }
// }