using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirVinyl.API.DbContexts;
using AirVinyl.API.EntityDataModels;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AirVinyl.API
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

            services.AddDbContext<AirVinylDbContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=AirVinylDB;Trusted_Connection=True;");
            });

            services.AddSingleton<AirVinylEntityDataModel>();

            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            AirVinylEntityDataModel airVinylEntityDataModel)
        {
            if (env.IsDevelopment()) 
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseODataBatching();

            app.UseRouting();

            app.UseAuthorization(); 

            var batchHandler = new DefaultODataBatchHandler();
            batchHandler.MessageQuotas.MaxNestingDepth = 3;
            batchHandler.MessageQuotas.MaxNestingDepth = 10;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapODataRoute(
                    "AirVinyl OData", 
                    "odata", 
                    airVinylEntityDataModel.GetEntityDataModel(), 
                    batchHandler)
                    .Select()
                    .Expand()
                    .OrderBy()
                    .MaxTop(10)
                    .Count()
                    .Filter();
            }); 
        }
    }
}
