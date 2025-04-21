namespace EclipseWorks.Audit.Messages
{
    public class TaskDataMessage
    {
        public Guid ReferenceKey { get; set; }
        public TaskPriorityEnum Priority { get; set; }
        public TaskStatusEnum Status { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public DateTime? UpdateAt { get; set; }
        public Guid ProjectId { get; set; }
    }

}
