using EclipseWorks.Application.Project.Common;
using EclipseWorks.Application.Project.CreateProject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Project.UpdateTask
{
    public class UpdateTaskCommand : IRequest<UpdateTaskResult>
    {
        public Guid ProjectId { get; set; }
        public Guid ReferenceKey { get; set; }
        public TaskStatusEnum Status { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }
}
