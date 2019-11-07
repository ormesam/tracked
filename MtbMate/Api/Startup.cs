using System.Text;
using DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddOptions();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            var settingsSection = Configuration.GetSection("AppSettings");
            var settings = settingsSection.Get<AppSettings>();

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => {
                   options.TokenValidationParameters = new TokenValidationParameters {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = false,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = "samorme.com",
                       ValidAudience = "samorme.com",
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecurityKey)),
                   };
               });

            services.AddDbContext<ModelDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
