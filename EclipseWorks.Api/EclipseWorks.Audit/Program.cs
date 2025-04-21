using EclipseWorks.Audit;
using EclipseWorks.Audit.Infrastructure.Dao;
using EclipseWorks.Audit.Infrastructure.RabbitMq;
using EclipseWorks.Audit.Migrations;
using FluentMigrator.Runner;
using RabbitMQ.Client;
using static System.Formats.Asn1.AsnWriter;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ProjectCreatedWorker>();
builder.Services.AddHostedService<ProjectDeletedWorker>();
builder.Services.AddHostedService<TaskCreatedWorker>();
builder.Services.AddHostedService<TaskChangedWorker>();
builder.Services.AddHostedService<TaskDeletedWorker>();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<ProjectMessageDao>();
builder.Services.AddSingleton<TaskMessageDao>();


builder.Services.AddSingleton(rabbitmq =>
{
    var rabbitMqUrl = builder.Configuration.GetConnectionString("RabbitMq");
    var factory = new ConnectionFactory() { HostName = rabbitMqUrl };

    return new RabbitMqConnector(factory);
});

var serviceCollection = builder.Services
        .AddLogging(c => c.AddFluentMigratorConsole())
        .AddFluentMigratorCore()
        .ConfigureRunner(c => c
            .AddPostgres15_0()
            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("PostgreSql"))
            .ScanIn(AppDomain.CurrentDomain.Load("EclipseWorks.Audit")).For.All());
var serviceProvider = serviceCollection.BuildServiceProvider(false);

IHost host = builder.Build();
host.Migrate(serviceProvider);
host.Run();
