using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Audit.Migrations
{
    public static class MigrationManager
    {
        public static IHost Migrate(this IHost app, ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
                runner.ListMigrations();
                runner.MigrateUp(20250421000001);
            }
            return app;
        }
    }
}
