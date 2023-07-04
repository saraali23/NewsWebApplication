using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NewsAPI.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using NewsAppClasses;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NewsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Database

            builder.Services.AddDbContext<MainDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("NewsDB") ?? throw new InvalidOperationException("Connection string 'MainDBContext' not found.")));

            builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling =
               Newtonsoft.Json.ReferenceLoopHandling.Ignore);

         


            #endregion


            #region Cors
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    //any means any thing, it can be specified if i want to
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            #endregion

            #region Identity Managers

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<MainDBContext>();

            #endregion

            #region Authentication
            builder.Services.AddAuthentication(options =>
            {
                //Used Authentication Scheme
                options.DefaultAuthenticateScheme = "CoolAuthentication";

                //Used Challenge Authentication Scheme
                options.DefaultChallengeScheme = "CoolAuthentication";
            }).AddJwtBearer("CoolAuthentication", options =>
            {
                var secretKeyString = builder.Configuration.GetValue<string>("SecretKey") ?? string.Empty;
                var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString);
                var secretKey = new SymmetricSecurityKey(secretKeyInBytes);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            #endregion

            #region Authoriation
            builder.Services.AddAuthorization(options =>
            {

                options.AddPolicy("AllowAdmins", policy => policy
                    .RequireClaim(ClaimTypes.Role, "Admin"));
            });

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.UseCors(MyAllowSpecificOrigins);
            app.Run();
        }
    }
}