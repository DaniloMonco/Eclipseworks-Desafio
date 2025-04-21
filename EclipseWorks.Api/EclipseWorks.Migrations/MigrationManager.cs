using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace EclipseWorks.Migrations
{
    public static class MigrationManager
    {
        public static IApplicationBuilder Migrate(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
                runner.ListMigrations();
                runner.MigrateUp(20250420000001);
                runner.MigrateUp(20250420000002);
                
            }
            return app;
        }
    }
}
