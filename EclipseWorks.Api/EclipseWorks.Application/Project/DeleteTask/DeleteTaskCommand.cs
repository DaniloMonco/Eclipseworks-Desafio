using EclipseWorks.Application.Project.UpdateTask;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Project.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public Guid ProjectId { get; set; }
        public Guid ReferenceKey { get; set; }
    }

}
