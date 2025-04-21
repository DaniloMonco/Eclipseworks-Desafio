using EclipseWorks.Api.Middleware;
using EclipseWorks.Application.Project.CreateProject;
using EclipseWorks.Domain.DAO;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using EclipseWorks.Infrastructure.Common;
using EclipseWorks.Infrastructure.Dao;
using EclipseWorks.Infrastructure.Events;
using EclipseWorks.Infrastructure.Repository;
using EclipseWorks.Migrations;
using FluentMigrator.Runner;
using RabbitMQ.Client;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("EclipseWorks.Application")));

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReportDao, ReportDao>();

builder.Services.AddSingleton<IPublisher, RabbitMqPublisher>();
builder.Services.AddSingleton(rabbitmq =>
{
    var rabbitMqUrl = builder.Configuration.GetConnectionString("RabbitMq");
    var factory = new ConnectionFactory() { HostName = rabbitMqUrl };

    return new RabbitMqConnector(factory);
});


builder.Services
        .AddLogging(c => c.AddFluentMigratorConsole())
        .AddFluentMigratorCore()
        .ConfigureRunner(c => c
            .AddPostgres15_0()
            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("PostgreSql"))
            .ScanIn(AppDomain.CurrentDomain.Load("EclipseWorks.Migrations")).For.All());


WebApplication app = builder.Build();
app.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
