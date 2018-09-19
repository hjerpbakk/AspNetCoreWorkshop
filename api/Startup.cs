using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using api.Services;

namespace api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHighScoreService, HighScoreService>();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // The browser's same-origin policy makes sure that you can only request resources from the
            // server hosting the page you're visiting.
            
            // Normally, this is very good, as it stops malicious pages from making requests on your behalf,
            // but since our API is now running on a separate server, it will break our application
            // if we don't account for it.

            // For testing purposes, we simply allow any request coming from our PC -
            // for a larger application, a better way to handle this is to set up a "reverse proxy" like Traefik or Nginx,
            // which handles requests from the user, and passes them on to the correct container.
            app.UseCors(builder => builder.WithOrigins("http://localhost").AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();
        }
    }
}
