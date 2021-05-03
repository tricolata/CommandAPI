using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace CommandAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration {get;}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Read the connection string from appsetting.json
            services.AddTransient<IDbConnection>((sp) => new MySqlConnection(this.Configuration.GetConnectionString("MySQLConnection")));

            // With secret key 
            var builder = new MySqlConnectionStringBuilder();
            builder.ConnectionString = 
                Configuration.GetConnectionString("MySQLConnection");
                builder.UserID= Configuration["Uid"];
                builder.Password = Configuration["Pwd"];
            
            services.AddTransient<IDbConnection>((sp) => new MySqlConnection(builder.ConnectionString));

            services.AddControllers();
            //services.AddScoped<ICommandAPIRepo, MockCommandAPIRepo>();  // only as mock. Delete once real repo is set up
            services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();   // real one
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
