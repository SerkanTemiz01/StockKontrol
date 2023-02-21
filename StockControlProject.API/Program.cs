using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StockControlProject.Repository.Abstract;
using StockControlProject.Repository.Concrete;
using StockControlProject.Repository.Context;
using StockControlProject.Service.Abstract;
using StockControlProject.Service.Concrete;

namespace StockControlProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers().AddNewtonsoftJson(option=>
            option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StockControlContext>(_ =>
            {
                _.UseSqlServer("Server=LAPTOP-H9PBUUIN\\MSSQLSERVER2019;database=NRM1StokKontrol;Trusted_Connection=True;");
            });

            builder.Services.AddTransient(typeof(IGenericService<>),typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            var app = builder.Build();


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