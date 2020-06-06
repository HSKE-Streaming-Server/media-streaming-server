using System;
using System.Collections.Generic;
using System.Linq;
using API.Login;
using API.ExceptionHandling;
using API.Manager;
using MediaInput;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transcoder;

namespace API
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(opt => {
                opt.InvalidModelStateResponseFactory = (CustomErrorResponse);
            });
            services.AddSingleton<ServerManager>();
            //Add transcoder and grabber as dependency injection so we can in turn inject the logger into them
            services.AddSingleton<FFmpegAsProcess>();
            services.AddSingleton<Grabber>();
            services.AddSingleton<LoginDbHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //Modelvalidation builds automatic Error responses. That's why we customize here our own response, that all error responses are build up the same way.
        private BadRequestObjectResult CustomErrorResponse(ActionContext actionContext) {
             return new BadRequestObjectResult(actionContext.ModelState  
            .Where(modelError => modelError.Value.Errors.Count > 0)  
            .Select(modelError => new Error {  
                ErrorMessage = modelError.Value.Errors.FirstOrDefault()?.ErrorMessage  
            }).First());
        }
    }
}
