namespace EclipseWorks.Api.Features.Report.AvgTasksCompletedUser
{
    public record GetAvgTaskCompletedItem(Guid UserId, decimal AvgTaskCount);
}
