using EUDBLD_HFT_2023241.Logic;
using EUDBLD_HFT_2023241.Models;
using EUDBLD_HFT_2023241.Repository;
using EUDBLD_HFT_2023241.Repository.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Endpoint
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
            services.AddTransient<DartsDbContext>();

            services.AddTransient<IRepository<Player>, PlayerRepository>();
            services.AddTransient<IRepository<PlayerChampionship>, PlayerChampionshipRepository>();
            services.AddTransient<IRepository<Championship>, ChampionshipRepository>();
            services.AddTransient<IRepository<Prizes>, PrizesRepository>();

            services.AddTransient<IPlayerLogic, PlayerLogic>();
            services.AddTransient<IPlayerChampionshipLogic, PlayerChampionshipLogic>();
            services.AddTransient<IChampionshipLogic, ChampionshipLogic>();
            services.AddTransient<IPrizeLogic, PrizeLogic>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EUDBLD_HFT_2023241.Endpoint", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EUDBLD_HFT_2023241.Endpoint v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
