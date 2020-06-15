using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payment.Gateway.Data;
using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Application.HttpClient;
using Payment.Gateway.Application.Services;
using Payment.Gateway.Data.Repositories;
using Payment_Gateway.Services;
using Polly;
using Polly.Extensions.Http;

namespace Payment_Gateway
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
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Web Api", Version = "V1" });
            }); 
            
            services.AddHttpClient("payment", c =>
            {
                c.BaseAddress = new Uri("https://localhost:5001");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddPolicyHandler(GetRetryPolicy());


            services.AddDbContext<PaymentContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PaymentConnection")));
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICardDetailsRepository, CardDetailsRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IPaymentManager, PaymentManager>();
            services.AddScoped<ICardDetailsService, CardDetailsService>();
            services.AddScoped<ITransactionService,TransactionService>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddSingleton<IApiClient, ApiClient>();



        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
           return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Api");
                //c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
