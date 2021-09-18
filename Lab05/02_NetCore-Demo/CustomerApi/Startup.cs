using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CustomerApi
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

            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerApi", Version = "v1" });
            });
            
           
            services.Configure<Config.MqConfig>(Configuration.GetSection(nameof(Config.MqConfig)));
            services.Configure<Config.MongoConfig>(Configuration.GetSection(nameof(Config.MongoConfig)));

            services.AddSingleton<Config.IMongoConfig>(sp =>sp.GetRequiredService<IOptions<Config.MongoConfig>>().Value);
            services.AddSingleton<Config.IMqConfig>(sp =>sp.GetRequiredService<IOptions<Config.MqConfig>>().Value);

            services.AddSingleton<Repository.ICustomerRepository, Repository.CustomerRepository>();

            services.AddSingleton<Service.ICustomerService, Service.CustomerService>();
            services.AddSingleton<Service.IBusService, Service.BusService>();

            services.AddHostedService<Service.SubscribeToCompanyConsumer>();

            services.AddHealthChecks();
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
