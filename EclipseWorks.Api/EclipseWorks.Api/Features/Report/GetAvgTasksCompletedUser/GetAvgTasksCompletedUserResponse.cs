namespace EclipseWorks.Api.Features.Report.AvgTasksCompletedUser
{
    public record GetAvgTasksCompletedUserResponse(int LastDays, IEnumerable<GetAvgTaskCompletedItem> Items)
    {
    }
}
