using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Services;
using Newtonsoft.Json;
using Helpers;
using System.IO;
using Models;
using System.Threading.Tasks;
using Providers;
using Extensions;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //auth
            // services.AddDistributedMemoryCache();

            // services.Configure<IISOptions>(options =>
            // {
            //     options.AutomaticAuthentication = false;
            // });

            // services.AddSession(options =>
            // {
            //     // Set a short timeout for easy testing.
            //     options.IdleTimeout = TimeSpan.FromSeconds(10);
            // });

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddScoped<EmailService>();
            services.AddScoped<AccountService>();
            services.AddScoped<TokkenHandler>();
            services.AddScoped<HtmlService>();

            //Provide a secret key to Encrypt and Decrypt the Token
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               //    x.Events = new JwtBearerEvents
               //    {
               //        OnTokenValidated = context =>
               //        {
               //            //    var userService = context.HttpContext.RequestServices.GetRequiredService/*<IUtilisateurService>*/();
               //            //    var userId = int.Parse(context.Principal.Identity.Name);
               //            //    var user = userService.GetById(userId);
               //            //    if (user == null)
               //            //    {
               //            //        // return unauthorized if user no longer exists
               //            //        context.Fail("Unauthorized");
               //            //    }
               //            return Task.CompletedTask;
               //        }
               //    };
               /**
               * this just for the sake of signalR
               */
               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       var accessToken = context.Request.Query["access_token"];
                       if (string.IsNullOrEmpty(accessToken) == false)
                       {
                           context.Token = accessToken;
                       }
                       return Task.CompletedTask;
                   }
               };

               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });
            //auth

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                   .WithOrigins(
                       "http://localhost:4201",
                       "http://localhost:4200",
                       "http://192.168.1.25:4202",
                       "http://192.168.43.238:4202"
                       )
                           // .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            services.AddDbContext<MyContext>(options =>
            {
                // options.UseSqlServer(Configuration.GetConnectionString("db"));
                options.UseSqlite(Configuration.GetConnectionString("sqlite"));

                options.EnableSensitiveDataLogging(true);
            });

            // services.AddControllersWithViews()

            services.AddControllers();

            // services.AddControllers().AddNewtonsoftJson(options =>
            // {
            //     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            // });

            // services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //auth
            //Addd Utilisateur session - JRozario

            // app.UseSession();
            //Add JWToken to all incoming HTTP Request Header
            // app.Use(async (context, next) =>
            // {
            //     var JWToken = context.Session.GetString("JWToken");
            //     if (!string.IsNullOrEmpty(JWToken))
            //     {
            //         context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
            //     }
            //     await next();
            // });

            //Add JWToken Authentication service - JRozario
            // app.UseAuthentication();
            //auth


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           else
            {
                app.Use(async (context, next) =>
                {
                    await next();

                    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/index.html";
                        await next();
                    }
                });
            }





            app.UseCors("CorsPolicy");
            app.UseMiddleware<ErrorHandler>();
            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                // endpoints.MapHub<ChatHub>("/ChatHub");

                // endpoints.MapFallbackToFile("/index.html");
            });


        }
    }
}


// <modules runAllManagedModulesForAllRequests="false">
//     <remove name="WebDAVModule" />
//   </modules>

// https://stackoverflow.com/questions/48188895/asp-net-core-with-iis-http-verb-not-allowed