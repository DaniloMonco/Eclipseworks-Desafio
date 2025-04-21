namespace EclipseWorks.Audit.Messages
{
    public class TaskBaseMessage
    {
        public DateTime TimeStamp { get; set; }
        public TaskDataMessage Data { get; set; }
        public Guid UserId { get; set; }
    }
}
