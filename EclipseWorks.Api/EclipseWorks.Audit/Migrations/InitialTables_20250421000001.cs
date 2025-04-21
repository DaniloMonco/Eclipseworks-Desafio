using FluentMigrator;

namespace EclipseWorks.Migrations
{
    [Migration(20250420000001)]
    public class InitialTables_20250421000001 : Migration
    {
        public override void Down()
        {
            Delete.Table("TaskAudit");
            Delete.Table("ProjectAudit");
        }

        public override void Up()
        {
            Create.Table("ProjectAudit")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("OccurredIn").AsDateTime().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Action").AsInt32().NotNullable()
                .WithColumn("ProjectId").AsGuid().NotNullable()
                .WithColumn("ProjectName").AsString(100).NotNullable()
                .WithColumn("ProjectDescription").AsString(250).NotNullable()
                .WithColumn("ProjectUserId").AsGuid().NotNullable();


            Create.Table("TaskAudit")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("OccurredIn").AsDateTime().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Action").AsInt32().NotNullable()

                .WithColumn("TaskReferenceKey").AsGuid().NotNullable()
                .WithColumn("TaskPriority").AsInt32().NotNullable()
                .WithColumn("TaskStatus").AsInt32().NotNullable()
                .WithColumn("TaskName").AsString(100).NotNullable()
                .WithColumn("TaskComments").AsString(250).NotNullable()
                .WithColumn("TaskUpdateAt").AsDateTime().Nullable()
                .WithColumn("TaskProjectId").AsGuid().NotNullable();
        }
    }
}
