namespace EclipseWorks.Audit.Messages
{
    public abstract class ProjectBaseMessage
    {
        public DateTime TimeStamp { get; set; }
        public ProjectDataMessage Data { get; set; }
        public Guid UserId { get; set; }

    }
}
