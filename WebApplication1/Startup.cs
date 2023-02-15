using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Thoughts.Core.ConfigModels;
using Thoughts.Core.Interfaces;
using Thoughts.Core.Services;
using Thoughts.Domain.Entities;
using Thoughts.Infrastructure.Data;
using Thoughts.Infrastructure.Repositories;
using Logistics.API.ExceptionHandling;
using Thoughts.API.Middleware;

namespace Thoughts.API0
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<ThoughtDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ThoughtDbConnectionString"))
            .UseLazyLoadingProxies());
			services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 7;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ThoughtDataContext>().AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthTokenKey"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });
			services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<AppSettings>(Configuration);
            services.AddScoped<TokenService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IValidationService, ValidationService>();


			services.AddScoped<IUserService, UserService>();
            services.AddScoped<IThoughtService, ThoughtService>();
            services.AddScoped<IThoughtRepository, ThoughtRepository>();
			services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<ILikesRepository, LikesRepository>();
			services.AddScoped<IThoughtCommentService, ThoughtCommentService>();
			services.AddScoped<IThoughtCommentRepository, ThoughtCommentRepository>();


			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
					policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000").AllowCredentials();
				});
            });

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add<ExceptionFilter>();

            });

			services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "JWTToken_Auth_API",
					Version = "v1"
				});
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
			});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
			if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

			app.UseMiddleware<HttpContextMiddleware>();

			app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseCors("CorsPolicy");
            app.UseAuthentication();
			app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
