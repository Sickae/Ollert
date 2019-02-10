using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using Ollert.DataAccess;
using Ollert.Logic.DTOs.Mappings;
using Ollert.Logic.Managers;
using Ollert.Logic.Managers.Interfaces;
using Ollert.Logic.Repositories;
using Ollert.Web.Models.Mappings;
using System.Linq;
using System.Text;

namespace Ollert
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // IOC
            services.AddSingleton(SessionFactoryCreator.BuildConfiguration(_configuration.GetConnectionString("ollert"))
                .BuildSessionFactory());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(x => x.GetServices<ISessionFactory>().First().OpenSession());

            // manager osztályok
            services.AddScoped<IBoardManager, BoardManager>();
            services.AddScoped<ICardListManager, CardListManager>();
            services.AddScoped<ICardManager, CardManager>();
            services.AddScoped<ICommentManager, CommentManager>();
            services.AddScoped<ILabelManager, LabelManager>();

            // repository osztályok
            services.AddScoped<BoardRepository>();
            services.AddScoped<CardListRepository>();
            services.AddScoped<CardRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Board}/{action=BoardList}/{id?}");
            });

            // ISO-8859-2 támogatás
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            InitializeAutoMapper();
        }

        private static void InitializeAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperBasicProfile>();
                cfg.AddProfile<AutoMapperLogicProfile>();
                cfg.AddProfile<AutoMapperWebProfile>();
            });

            // TODO lehet nem fontos
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
