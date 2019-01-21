using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NHibernate;
using Ollert.DataAccess;
using Ollert.Logic.DTOs.Mappings;
using Ollert.Logic.Interfaces;
using Ollert.Web.Infrastructure.Log;
using Ollert.Web.Models.Mappings;
using Serilog.Core.Enrichers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromMinutes(5);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RequestLocalizationOptions>(opt =>
            {
                var enCulture = new CultureInfo("en-US");
                var huCulture = new CultureInfo("hu-HU");
                var supportedCultures = new[]
                {
                    enCulture,
                    huCulture
                };

                opt.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new CustomRequestCultureProvider(context =>
                    {
                        return Task.FromResult(new ProviderCultureResult(huCulture.Name));
                    })
                };

                opt.DefaultRequestCulture = new RequestCulture(enCulture.Name);
                opt.SupportedCultures = supportedCultures;
                opt.SupportedUICultures = supportedCultures;
            });

            services.AddSingleton(SessionFactoryCreator.BuildConfiguration(_configuration.GetConnectionString("ollert"))
                .BuildSessionFactory());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAppContext, Web.Infrastructure.AppContext>();
            services.AddScoped(x => x.GetServices<ISessionFactory>().First().OpenSession());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UserSerilogLogContext(opt =>
            {
                opt.EnrichersForContextFactory = appContext => new[]
                {
                    new PropertyEnricher("CorrelationId", appContext.CorrelationId)
                };
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(locOptions.Value);
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
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
