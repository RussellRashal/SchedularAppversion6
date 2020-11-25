using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Schedular.API.Data;
using Schedular.API.Helpers;
using Schedular.API.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Schedular.API
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
            services.AddDbContext<DataContext>(x => x.UseMySql(Configuration
                .GetConnectionString("DefaultConnection"),
                        new MySqlServerVersion(new Version(8, 0, 21)), 
                        mySqlOptions => mySqlOptions
                            .CharSetBehavior(CharSetBehavior.NeverAppend)));
            //lockout options
            var lockoutOptions = new LockoutOptions()
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10),
                MaxFailedAccessAttempts = 5
            };
                
            // for role and identity authentication
            // initial creation of user in the user table in database 
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                // password requirements
                opt.Lockout = lockoutOptions;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            
            // allows api to use authentication 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });  


            // authorisation policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminAccess", policy => policy.RequireRole("Admin"));
                options.AddPolicy("everyone", policy => policy.RequireRole("Admin", "standard"));
            });

            services.AddControllers(options => 
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                
                options.Filters.Add(new AuthorizeFilter(policy));
            })
             .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = 
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }); 
            
            services.AddScoped<ITaskScheduleRepository, TaskScheduleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotesRepository, NotesRepository>();
            services.AddScoped<IAttachmentFileRepository, AttachmentFileRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddControllers();
            services.AddCors(); 
        


            //allows use of tokens
  

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);            
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
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            //app.UseHttpsRedirection();

            app.UseRouting();        
            
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
                .WithOrigins("http://localhost:4200"));  

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("index", "Fallback");
            });
        }
    }
}
