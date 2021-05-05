using AirVinyl.API.DbContexts;
using AirVinyl.EntityDataModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirVinyl
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
            var batchHandler = new DefaultODataBatchHandler();
            batchHandler.MessageQuotas.MaxNestingDepth = 3;
            batchHandler.MessageQuotas.MaxOperationsPerChangeset = 10;

            services.AddControllers()
                .AddOData(opt =>
                    opt.AddModel("odata",
                        new AirVinylEntityDataModel().GetEntityDataModel(), 
                        batchHandler)
                    .Select()
                    .Expand()
                    .OrderBy()
                    .SetMaxTop(10)
                    .Count()
                    .Filter());

            services.AddDbContext<AirVinylDbContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=AirVinylDemoDB;Trusted_Connection=True;")
                .EnableSensitiveDataLogging();
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseODataBatching();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
