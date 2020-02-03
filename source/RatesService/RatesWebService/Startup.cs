namespace RatesWebService
{
    using System;
    using System.Net.Http;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NHibernate;
    using NHibernate.Dialect;
    using OpenExchangeRatesSource;
    using RatesService;
    using RatesSources.Common;
    using RatesStorageService;
    using RatesWebService.Properties;

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
            this.RegisterNHibernate(services);
            this.RegisterRatesProvider(services);
            this.RegisterRatesService(services);
            services.AddSingleton<IRatesStorageService, RatesStorageService>();
            services.AddHttpClient();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void RegisterNHibernate(IServiceCollection serviceCollection)
        {
            var connectionString = this.Configuration["DB_ConnectionString"] ?? throw new InvalidOperationException(Resources.databaseConnectionStringError);
            serviceCollection.AddSingleton(Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(connectionString)
                    .Dialect<PostgreSQL82Dialect>().ShowSql())
                .Mappings(RatesStorageService.RegisterMapping)
                .BuildSessionFactory());
        }

        public void RegisterRatesProvider(IServiceCollection serviceCollection)
        {
            var appId = this.Configuration["RatesProviderID_OpenExchangeRates"] ?? throw new InvalidOperationException(Resources.openExchangeRatesAppIdProviderError);
            serviceCollection.AddSingleton<IRatesSourceProvider>(services => new OpenExchangeSourseProvider(services.GetService<IHttpClientFactory>(), appId));
        }

        public void RegisterRatesService(IServiceCollection serviceCollection)
        {
            var expiredMinutesString = this.Configuration["ExpiredMinutes"] ?? throw new InvalidOperationException(Resources.expiredTimeError);
            if (!int.TryParse(expiredMinutesString, out int expiredMinutes))
            {
                throw new InvalidCastException(Resources.expiredTimeCastError);
            }

            serviceCollection.AddSingleton<IRatesService>(services => new RatesService(
                services.GetService<IRatesSourceProvider>(),
                services.GetService<IRatesStorageService>(),
                expiredMinutes));
        }
    }
}
