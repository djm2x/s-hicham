// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Models;
// using Api.Providers;
// using Microsoft.AspNetCore.WebUtilities;
// using System.Text;

// namespace Controllers
// {
//     [Route("api/[controller]/[action]")]
//     [ApiController]
//     public class UtilisateursController : SuperController<Utilisateur>
//     {
//         public UtilisateursController(MyContext context) : base(context)
//         { }

//         [HttpGet("{startIndex}/{pageSize}/{sortBy}/{sortDir}/{email}/{idProfil}/{idTypeprofil}/{idRegion}/{departement}")]
//         public async Task<IActionResult> GetAll(int startIndex, int pageSize, string sortBy, string sortDir, string email, int idProfil, int idTypeprofil, int idRegion, string departement)
//         {
//             var q = _context.Utilisateurs
//                 .Where(e => email == "*" ? true : e.Email.ToLower().Contains(email.ToLower()))
// .Where(e => idProfil == 0 ? true : e.IdProfil == idProfil)
// .Where(e => idTypeprofil == 0 ? true : e.IdTypeprofil == idTypeprofil)
// .Where(e => idRegion == 0 ? true : e.IdRegion == idRegion)
// .Where(e => departement == "*" ? true : e.Departement.ToLower().Contains(departement.ToLower()))

//                 ;

//             int count = await q.CountAsync();

//             var list = await q.OrderByName<Utilisateur>(sortBy, sortDir == "desc")
//                 .Skip(startIndex)
//                 .Take(pageSize)

//                 .Select(e => new
//                 {
//                     id = e.Id,
//                     email = e.Email,
//                     password = e.Password,
//                     emailVerified = e.EmailVerified,
//                     isActive = e.IsActive,
//                     profil = e.Profil.Nom,
//                     idProfil = e.IdProfil,
//                     idTypeprofil = e.IdTypeprofil,
//                     region = e.Region.Nom,
//                     idRegion = e.IdRegion,
//                     departement = e.Departement,

//                 })
//                 .ToListAsync()
//                 ;

//             return Ok(new { list = list, count = count });
//         }

//     }
// }