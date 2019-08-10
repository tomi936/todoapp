using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Todos.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // for the routing of requests
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // adds all the database related component
            services.AddTodosDal(this.Configuration);

            // adds the caching related components
            services.AddTodosCache(this.Configuration);

            // only when run in debug mode: prefills the database with sample data
            if (HostingEnvironment.IsDevelopment())
                services.AddHostedService<TestDataProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // only when run in debug mode: allow CORS to make frontend development simpler
            if (env.IsDevelopment())
                app.UseCors(config => config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // for the routing of requests
            app.UseMvc();
        }
    }
}
