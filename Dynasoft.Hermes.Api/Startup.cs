using AutoMapper;
using Dynasoft.Hermes.Api.Extensions;
using Dynasoft.Hermes.Api.Settings;
using Dynasoft.Hermes.Infrastructure.Messaging;
using Dynasoft.Hermes.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Dynasoft.Hermes.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            services.Configure<RabbitSettings>(Configuration.GetSection("RabbitMq"));

            services.AddControllers();

            services.AddMvc().AddNewtonsoftJson();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hermes API", Version = "v1" });
            });

            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));

            //connecting to RabbitMQ
            var hostName = Configuration["RabbitMq:HostName"];
            var userName = Configuration["RabbitMq:UserName"];
            var password = Configuration["RabbitMq:Password"];

            services.AddRabbitMq(hostName, userName, password);
            
            services.AddScoped<IMessagePublisher, MessagePublisher>();
            services.SetUpSerilog();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hermes API V1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
