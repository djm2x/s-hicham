
// using Api.Providers;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.SignalR;
// using Microsoft.EntityFrameworkCore;
// using Models;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Diagnostics.CodeAnalysis;
// using System.Linq;
// using System.Threading.Tasks;


// namespace Hubs
// {
//     // public interface IChatHub : IClientProxy
//     // {
//     //     Task ReceiveMessage(Chat msg);
//     // }

//     [Authorize]
//     public class ChatHub : Hub //: Hub<IMessageHub>
//     {

//         // public async Task NewMessage(string groupName, Message msg)
//         // {
//         //     await Clients.Group(groupName).SendAsync("MessageReceived", msg);
//         // }
//         protected  MyContext _context;
//         public ChatHub(MyContext context)
//         {
//             _context = context;
//         }

//         // public string GetConnectionId()
//         // {
//         //     return Context.ConnectionId;
//         // }

//         // public async Task NewMessage(Message msg)
//         // {
//         //     await Clients.Others.SendAsync("MessageReceived", msg);
//         // }

//         // public async Task ListConnected(string something)
//         // {
//         //     await Clients.All.SendAsync("ConnectedUtilisateur", ConnectedUtilisateur.ConnectedUtilisateurs);
//         // }

//         public async Task JoinGroupPlace(string groupName)
//         {
//             await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
//         }

//         // public async Task<object> GetUnReadMessage3(int idReceiver)
//         // {
//         //     var list = await _context.Chats
//         //         .Where(e => e.IdReceiver == idReceiver && e.Lu == false)
//         //         .Include(e => e.Sender)
//         //         .ToListAsync()
//         //         ;

//         //     var res = list
//         //         // .Include(e => e.Sender)
//         //         // .GroupBy(e => new {id = e.IdSender, username = e.Sender.UtilisateurName})
//         //         .GroupBy(e => e.Sender)
//         //         .Select(e => new
//         //         {
//         //             sender = e.Key,
//         //             info = e,
//         //         })
//         //         // .Select(e => e.Key.UtilisateurName)
//         //         .ToList()
//         //         ;

//         //     return res;
//         // }

//         public async Task<object> GetUnReadMessage2(int idReceiver)
//         {
//             return await _context.Utilisateurs
//                 .Select(e => new
//                 {
//                     // sender = e.Receivers.GroupBy(g => g.IdSender).Select(s => new
//                     // {
//                     //     sender = s.Key,
//                     //     info = s.LastOrDefault()
//                     // }),
//                 })
//                 .ToListAsync()
//                 ;

//         }

//         // public async Task<object> GetUnReadMessage(int idReceiver)
//         // {
//         //     return await _context.Chats
//         //         .Where(e => e.IdReceiver == idReceiver && e.Lu == false)
//         //         .Include(e => e.Sender)
//         //         .OrderByDescending(e => e.Date)
//         //         .GroupBy(e => new
//         //         {
//         //             idSender = e.IdSender,
//         //             username = e.Sender.UtilisateurName,
//         //         })
//         //         .Select(e => new
//         //         {
//         //             idSender = e.Key.idSender,
//         //             username = e.Key.username,
//         //             count = e.Count(),
//         //         })
//         //         .ToListAsync()
//         //         ;
//         // }

//         public override async Task OnConnectedAsync()
//         {
//             try
//             {
//                 int idUtilisateur = int.Parse(Context.Utilisateur?.Identity?.Name);
//                 // var f = Context.Utilisateur?.Identity;
//                 var idPlace = int.Parse(Context.Utilisateur.Claims.SingleOrDefault(e => e.Type == "idPlace")?.Value);
//                 //  int idPlace = Context.GetPlaceUtilisateur();
//                 // ConnectedUtilisateur.ConnectedUtilisateurs.Add(idUtilisateur, new UtilisateurPlace { IdConnection = Context.ConnectionId, IdPlace = idPlace });
//                 ConnectedUtilisateur.ConnectedUtilisateurs2.Add(Context.ConnectionId,
//                     new UtilisateurPlace { IdConnection = Context.ConnectionId, IdUtilisateur = idUtilisateur, IdPlace = idPlace });


//                 // await Task.Delay(2000);
//                 await base.OnConnectedAsync();

//                 await Groups.AddToGroupAsync(Context.ConnectionId, idPlace + "");

//                 await Clients.Group(idPlace + "").SendAsync("toGroup", ConnectedUtilisateur.ConnectedUtilisateurs2.Values.Where(e => e.IdPlace == idPlace));

//                 // await Clients.All.SendAsync("ConnectedUtilisateur", ConnectedUtilisateur.List());
//                 // await Clients.Client(Context.ConnectionId).SendAsync("Notification", await GetUnReadMessage(idUtilisateur));

//             }
//             catch (System.Exception e)
//             {
//                 await Clients.All.SendAsync("InnerException", e.Message);
//             }
//             // await Clients.Client(Context.ConnectionId).SendAsync("ConnectedUtilisateur", ConnectedUtilisateur.List());
//         }


//         public override async Task OnDisconnectedAsync(Exception exception)
//         {
//             try
//             {
//                 // int idUtilisateur = int.Parse(Context.Utilisateur?.Identity?.Name);

//                 // ConnectedUtilisateur.ConnectedUtilisateurs.Remove(idUtilisateur);
//                 ConnectedUtilisateur.ConnectedUtilisateurs2.Remove(Context.ConnectionId);


//                 var idPlace = int.Parse(Context.Utilisateur.Claims.SingleOrDefault(e => e.Type == "idPlace")?.Value);

//                 await Groups.RemoveFromGroupAsync(Context.ConnectionId, idPlace + "");

//                 await Clients.Group(idPlace + "").SendAsync("toGroup", ConnectedUtilisateur.ConnectedUtilisateurs2.Values.Where(e => e.IdPlace == idPlace));


//                 await base.OnDisconnectedAsync(exception);

//                 // await Clients.All.SendAsync("ConnectedUtilisateur", ConnectedUtilisateur.List());

//             }
//             catch (System.Exception e)
//             {
//                 await Clients.All.SendAsync("InnerException", e.InnerException);
//             }
//         }
//     }

//     // public class MyEqualityComparer : IEqualityComparer<Chat>
//     // {
//     //     public bool Equals([AllowNull] Chat x, [AllowNull] Chat y)
//     //     {
//     //         return x.IdSender == y.IdSender;
//     //     }

//     //     public int GetHashCode([DisallowNull] Chat obj)
//     //     {
//     //         return obj.IdSender.GetHashCode();
//     //     }
//     // }

//     public static class ConnectedUtilisateur
//     {
//         // public static Dictionary<int, UtilisateurPlace> ConnectedUtilisateurs = new Dictionary<int, UtilisateurPlace>();
//         public static Dictionary<string, UtilisateurPlace> ConnectedUtilisateurs2 = new Dictionary<string, UtilisateurPlace>();

//         public static IEnumerable List()
//         {
//             // return ConnectedUtilisateur.ConnectedUtilisateurs.Select(u => new
//             // {
//             //     id = u.Key,
//             //     idConnection = u.Value.IdConnection,
//             //     idPlace = u.Value.IdPlace
//             // });

//             // var l = new ArrayList();

//             // ConnectedUtilisateur.ConnectedUtilisateurs.ToList().ForEach(e => 
//             // {
//             //     if (e.Value != null)
//             //     {
//             //         l.Add(new { id = e.Key, idConnection = e.Value.IdConnection, idPlace = e.Value.IdPlace });
//             //     }
//             // });

//             var l2 = new ArrayList();

//             ConnectedUtilisateur.ConnectedUtilisateurs2.ToList().ForEach(e =>
//             {
//                 if (e.Value != null)
//                 {
//                     l2.Add(new { idConnection = e.Key, id = e.Value.IdUtilisateur, idPlace = e.Value.IdPlace });
//                 }
//             });

//             return l2;
//         }
//     }

//     public class UtilisateurPlace
//     {
//         public string IdConnection { get; set; }
//         public int IdUtilisateur { get; set; }
//         public int IdPlace { get; set; }
//     }
// }