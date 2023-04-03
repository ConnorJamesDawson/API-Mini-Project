
using Microsoft.EntityFrameworkCore;
using NorthwindAPI_MiniProject.Data.Repository;
using NorthwindAPI_MiniProject.Models;

namespace NorthwindAPI_MiniProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var dbConnectionString = builder.Configuration["Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"];

            builder.Services.AddDbContext<NorthwindContext>(
                opt => opt.UseSqlServer(dbConnectionString));

            // Add services to the container.
            builder.Services.AddControllers()
            .AddNewtonsoftJson(
            opt => opt.SerializerSettings
            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services.AddScoped(
                typeof(INorthwindRepository<>),
                typeof(NorthwindRepository<>)); 

            builder.Services.AddScoped(
                typeof(INorthwindService<>),
                typeof(NorthwindService<>));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddScoped<INorthwindRepository<Customer>, CustomersRepository>();
            //builder.Services.AddScoped<INorthwindRepository<Product>, ProductsRepository>();
            builder.Services.AddScoped<INorthwindRepository<Order>, OrderRepository>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
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