
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using Microsoft.Extensions.Configuration;
using DAL.Interfaces;
using BLL.Interfaces;
using BLL.Services;
using AutoMapper;
using BLL;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<EventCatalogDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString(SettingStrings.EventCatalogDbConnection))
            );

            builder.Services.AddAuthentication();
            builder.Services.ConfigureIdentity();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddTransient<IEventService, EventService>();
            builder.Services.AddTransient<IParticipantService, ParticipantService>();
            builder.Services.AddTransient<ISpeakerService, SpeakerService>();
            
               
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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