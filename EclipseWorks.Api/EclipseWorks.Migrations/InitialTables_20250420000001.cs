using FluentMigrator;

namespace EclipseWorks.Migrations
{
    [Migration(20250420000001)]
    public class InitialTables_20250420000001 : Migration
    {
        public override void Down()
        {
            Delete.Table("UserRole");
            Delete.Table("Task");
            Delete.Table("Project");
        }

        public override void Up()
        {
            Create.Table("UserRole")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Position").AsInt32().NotNullable();


            Create.Table("Project")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Description").AsString(250).NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("UserRole", "Id");


            Create.Table("Task")
                .WithColumn("ReferenceKey").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Priority").AsInt32().NotNullable()
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Comments").AsString(250).NotNullable()
                .WithColumn("UpdateAt").AsDateTime().Nullable()
                .WithColumn("ProjectId").AsGuid().NotNullable().ForeignKey("Project", "Id");
        }
    }
}
