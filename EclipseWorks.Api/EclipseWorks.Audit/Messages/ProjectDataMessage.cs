﻿namespace EclipseWorks.Audit.Messages
{
    public class ProjectDataMessage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
    }

}
