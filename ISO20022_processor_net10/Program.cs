using ISO20022.Interfaces;
using ISO20022.Validator;
using Serilog;

namespace ISO20022_processor_net10
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMvc();

            builder.Services.AddSingleton<IXmlISOValidator, XmlISOValidator>();

            builder.Logging.ClearProviders();

            builder.Host.UseSerilog();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.Services.GetRequiredService<ILogger<Program>>().LogInformation("Starting ISO20022 processor...");
            var init = app.Services.GetRequiredService<IXmlISOValidator>();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}