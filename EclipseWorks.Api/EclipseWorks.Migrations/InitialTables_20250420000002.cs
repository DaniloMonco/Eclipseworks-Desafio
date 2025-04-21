using EclipseWorks.Domain.Entities;
using FluentMigrator;

namespace EclipseWorks.Migrations
{
    [Migration(20250420000002)]
    public class InitialTables_20250420000002 : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            var employee = new User(UserPositionEnum.Employee);
            Insert.IntoTable("UserRole")
                .Row(new {employee.Id, Position=(int)employee.Position});

            var manager = new User(UserPositionEnum.Manager);
            Insert.IntoTable("UserRole")
                .Row(new { manager.Id, Position =(int)manager.Position});
        }
    }
}
