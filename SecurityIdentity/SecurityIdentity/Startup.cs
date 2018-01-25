using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecurityIdentity.Data;
using SecurityIdentity.Models;
using SecurityIdentity.Services;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace SecurityIdentity
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
            StringBuilder connStringBuilder = new StringBuilder(Configuration.GetConnectionString("DefaultConnection"));
            connStringBuilder.Replace("{user}", Configuration["SqlServer:User"]);
            connStringBuilder.Replace("{password}", Configuration["SqlServer:Password"]);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connStringBuilder.ToString()));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add facebook login
            services.AddAuthentication()
                .AddFacebook((options) =>
                {
                    options.AppId = Configuration["Facebook:AppId"];
                    options.AppSecret = Configuration["Facebook:AppSecret"];
                });

            // requires HTTPS requests
            services.Configure<MvcOptions>((options) =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            //services.AddSingleton<ITodoItemService, FakeTodoItemService>();
            services.AddScoped<ITodoItemService, TodoItemService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            // create the database if not already exists
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();

                EnsureRolesAsync(roleManager).Wait();
                EnsureTestAdminAsync(userManager, Configuration).Wait();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            // set rewrite options
            var rewriteOptions = new RewriteOptions()
                .AddRedirectToHttpsPermanent();

            app.UseRewriter(rewriteOptions);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);

            if (alreadyExists) return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            string defaultAdminEmail = configuration["DefaultAdmin:Email"];

            var testAdmin = await userManager.Users
                .Where(x => x.UserName == defaultAdminEmail)
                .SingleOrDefaultAsync();

            if (testAdmin != null) return;

            testAdmin = new ApplicationUser {
                UserName = defaultAdminEmail,
                Email = defaultAdminEmail,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var a = await userManager.CreateAsync(testAdmin, configuration["DefaultAdmin:Password"]);
            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
        }
    }
}
