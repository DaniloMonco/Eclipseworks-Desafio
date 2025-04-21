namespace EclipseWorks.Api.Features.Project.CreateProject
{
    public class CreateProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid UserId { get; set; }
    }
}
