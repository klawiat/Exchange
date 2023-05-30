using Exchange.Api.Services;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

namespace Exchange.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Exchange.Api", Version = "v1" });
            });
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ExchangeRateService>();
            WebApplication app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint("v1/swagger.json", "Exchange.Api");
                });
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Exchange}/{action=GetExchangeRate}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    name: "convertion",
                    pattern: "{controller=Exchange}/{action=Convert}/{from}/{to}/{amount?}"
                    );
            });

            app.Run();
        }
    }
}