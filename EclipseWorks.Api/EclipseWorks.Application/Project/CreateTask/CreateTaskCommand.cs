using EclipseWorks.Application.Project.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Project.CreateTask
{
    public class CreateTaskCommand : IRequest<CreateTaskResult>
    {
        public Guid ProjectId { get; set; }
        public TaskPriorityEnum Priority { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }
}
