using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Handlers;
using FRBackend.Helpers;
using FRBackend.Interface;
using FRBackend.Requirements;
using FRBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FRBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Rewrite;

namespace FRBackend
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
            var connectionString = Configuration.GetSection("ConnectionString").Value;
            services.AddDbContext<FinancialReportDBContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = TokenHelper.Issuer,
                            ValidAudience = TokenHelper.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(TokenHelper.Secret)),
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyNonBlockedCustomer", policy =>
                {
                    policy.Requirements.Add(new CustomerBlockedStatusRequirement(false));

                });
            });

            services.AddSingleton<IAuthorizationHandler, CustomerBlockedStatusHandler>();

            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDesignationService, DesignationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleAssignService, RoleAssignService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ICommonCodeService, CommonCodeService>();
            services.AddScoped<IRoleWiseMenuAssignService, RoleWiseMenuAssignService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IHeaderService, HeaderService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IUserWiseUnitPermissionService, UserWiseUnitPermissionService>();
            services.AddScoped<IDocumentCategoryService, DocumentCategoryService>();

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleAssignRepository, RoleAssignRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<ICommonCodeRepository, CommonCodeRepository>();
            services.AddScoped<IMenuPermissionRepository, MenuPermissionRepository>();
            services.AddScoped<IRoleWiseMenuAssignRepository, RoleWiseMenuAssignRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUserSessionRepository, UserSessionRepository>();
            services.AddScoped<IHeaderRepository, HeaderRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserWiseUnitPermissionRepository, UserWiseUnitPermissionRepository>();
            services.AddScoped<IDocumentCategoryRepository, DocumentCategoryRepository>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                //.AddJsonOptions(jsonOptions =>
                //{
                //    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                //    jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                //    jsonOptions.JsonSerializerOptions.WriteIndented = false;
                //})
                ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FRBackend", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      builder =>
            //                      {
            //                          builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            //                      });
            //});
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder => builder
                 .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FRBackend v1"));
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);
            //app.UseCors();

            app.UseStaticFiles();

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FRBackend v1");
                    c.RoutePrefix = "swagger";
                });
            }
            if (env.IsProduction())
            {
                var option = new RewriteOptions();
                option.AddRedirect("^$", "/swagger/index.html");
                app.UseRewriter(option);
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(MyAllowSpecificOrigins);
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
