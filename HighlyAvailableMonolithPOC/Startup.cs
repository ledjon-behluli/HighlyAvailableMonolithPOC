using DotNetCore.CAP;
using HighlyAvailableMonolithPOC.Application.Pipelines;
using HighlyAvailableMonolithPOC.Infrastructure;
using HighlyAvailableMonolithPOC.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HighlyAvailableMonolithPOC
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
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsPipeline<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,>));

            services.AddSingleton<FileStore>();

            string connectionString = Configuration.GetValue<string>("ConnectionString");

            services.AddScoped(x => new SqlConnectionFactory(connectionString));
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connectionString), ServiceLifetime.Scoped);

            services.AddTransient<InitDeleteFolderHandler>();
            services.AddCap(x =>
            {
                x.UseEntityFramework<ApplicationDbContext>();
                x.UseRabbitMQ(options =>
                {
                    options.HostName = "localhost";
                    options.UserName = "admin";
                    options.Password = "admin";
                });
                x.UseDashboard();
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HighlyAvailableMonolith", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCapDashboard();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HighlyAvailableMonolith v1"));
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
