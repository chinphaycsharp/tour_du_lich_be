using AutoMapper;
using EPS.API.Helpers;
using EPS.API.HubConfig;
using EPS.API.Models;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Email;
using EPS.Service.Helpers;
using EPS.Utils.Repository.Audit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EPS.API
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
            services.AddAutoMapper(typeof(EPSBaseService));
            services.AddDbContext<EPSContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("EPS")));

            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();

            // using Microsoft.AspNetCore.DataProtection;
            services.AddDataProtection()
                .PersistKeysToDbContext<EPSContext>();
            services.AddSignalR();
            services.Configure<Audience>(Configuration.GetSection("Audience"));

            //services.AddIdentity<USERS, MASTER_DATAS>();
            services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<EPSContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 3;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(20);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                //options.User.RequireUniqueEmail = true;
            });
            services.Configure<SMTPConfigModel>(Configuration.GetSection("EmailConfiguration"));
            services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
            services.AddScoped(x => x.GetService<IHttpContextAccessor>().HttpContext.User);

            //register service
            services.AddScoped<IUserIdentity<int>, UserIdentity>();
            services.AddScoped<EPSRepository>();
            services.AddScoped<EPSBaseService>();
            services.AddScoped<AuthorizationService>();
            services.AddScoped<LookupService>();
            services.AddScoped<EmailService>();
            services.AddScoped<ImageTourService>();
            services.AddScoped<TourService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<RegisterTourService>();
            services.AddScoped<EvaluateTourService>();
            services.AddScoped<HotelService>();
            ConfigureJwtAuthService(services);

            services.AddMvc(x => x.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson();

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }
                    return Task.FromResult(0);
                };
            });

            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .WithOrigins("http://localhost:4200", "http://localhost:60531", "http://localhost:4100", "http://localhost:5200", "http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/chat");
            });
            app.UseAuthentication();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();

            // init data db fisrt run
            DbInitializer.Initialize(app.ApplicationServices);
        }

        public void ConfigureJwtAuthService(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audienceConfig["Aud"],

                // Validate the token expiry
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //o.Authority = "";
                //o.Audience = "";
                o.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }

    public class Lazier<T> : Lazy<T> where T : class
    {
        public Lazier(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        {
        }
    }
}
