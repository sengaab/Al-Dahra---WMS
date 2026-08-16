
using Microsoft.EntityFrameworkCore;
using whm.Models;

namespace whm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================
            // Database
            // =========================

            builder.Services.AddDbContext<DataBaseContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));


            // =========================
            // Add services
            // =========================

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();


            // =========================
            // Build App
            // =========================

            var app = builder.Build();


            // =========================
            // HTTP Request Pipeline
            // =========================

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
