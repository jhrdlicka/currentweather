using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using currentweather.Models;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace currentweather
{


    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(
                                          "http://hrdlicky.eu"
                                          , "http://localhost:53771"
                                          //                                          ,"http://hrdlicky.aspifyhost.com"
                                          )
                                      .AllowAnyHeader()
                                      .AllowCredentials()
                                      .AllowAnyMethod();
                                  });
            });

            services.AddDbContext<CurrentWeatherContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //  services.AddDbContext<CurrentWeatherContext>(opt =>
            //   opt.UseInMemoryDatabase("CurrentWeather"));

            //services.AddDefaultIdentity<IdentityUser>(
            //    options => options.SignIn.RequireConfirmedAccount = true)
            //    //                .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<CurrentWeatherContext>();

            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    //If you're using Chrome against localhost, you may have run into a change in Chrome cookie-handling behaviour.
                    //To verify, navigate to chrome://flags/ and change "Cookies without SameSite must be secure" to "Disabled".

                    options.LoginPath = "/account/google-login"; // Must be lowercase
                    options.SlidingExpiration = true;
                })
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection = Configuration.GetSection("Google");
                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];

               });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AllowedUsersOnly", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        //Here you can get many resouces from context, i get a claim here for example
                        var name = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

                        //write your logic to check user name .
                        var email = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                        if (email == "hrdlicka.jan@gmail.com")
                            return true;
                        if (email == "bhrdlickova@gmail.com")
                            return true;

                        return false;
                    });
                });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
