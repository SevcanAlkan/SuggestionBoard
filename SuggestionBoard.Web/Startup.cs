using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuggestionBoard.Data;
using SuggestionBoard.Data.Service;
using SuggestionBoard.Data.SubStructure;
using SuggestionBoard.Web.Helper;
using SuggestionBoard.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace SuggestionBoard.Web
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
            #region MVC Configuration

            services.AddControllersWithViews();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest)
               .AddRazorPagesOptions(options =>
               {
                   //options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                   //options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
               });
            #endregion

            #region .Net Identity

            services.AddIdentity<User, Role>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequiredUniqueChars = 1;
                config.Password.RequireUppercase = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireNonAlphanumeric = true;

                // Gerekirse kullan
                // options.Cookies.ApplicationCookie.LoginPath = new PathString("/Admin/Account/Login");

            }).AddEntityFrameworkStores<SuggestionBoardDbContext>();


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Authentication/Login";
                options.LogoutPath = $"/Authentication/Logout";
                options.AccessDeniedPath = $"/Authentication/AccessDenied";
            });

            #endregion

            #region AutoMapper Configuration

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            #endregion

            #region Dependency Injection          

            services.AddSingleton(mapper);
            services.AddDbContext<SuggestionBoardDbContext>(db =>
                db.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                                .Replace("{HostMachineIpAddress}", GetHostMachineIP.Get())));
            services.AddScoped<UnitOfWork>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<ISuggestionService, SuggestionService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISuggestionReactionService, SuggestionReactionService>();
            services.AddTransient<ISuggestionCommentService, SuggestionCommentService>();
            services.AddTransient(typeof(IBaseService<,,>), typeof(BaseService<,,>));

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "StaticFiles")),
                RequestPath = "/StaticFiles",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={600}");
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
