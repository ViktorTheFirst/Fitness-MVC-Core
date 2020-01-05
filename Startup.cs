using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessManagment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessManagment
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //instead of creating new instance of AppDbContext the "ASP.NET" Core checks if there is already 
            //some instance avaible in the pool
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("FitnessDBConnection")));

            //adds required Identity services to the application
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            //use this service to override the PASWORD complexity rules 
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
            });

            //the meaning of this authorization policy:
            //to reach any controller or action on the application the users must be authenticated
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();


            //if someone like the HomeController requests the ICustomerRepository service then create instance of MockPictureRepository
            //class and inject that instance
            services.AddScoped<ICustomerRepository, SQLCustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //this placeholder is automaticlly receives the non-success status code: "/Error/404"
                //this component reExecutes the request with this url: "/Error/404"
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
           
            
            app.UseStaticFiles(); //will serve static files that are present in wwwroot folder

            //we must authenticate users before serving the html views
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name:"Default", template:"{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
