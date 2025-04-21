using EclipseWorks.Application.Project.DeleteTask;
using EclipseWorks.Application.Project.GetProjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Project.GetTasksByProject
{
    public class GetTasksByProjectQuery : IRequest<IEnumerable<GetTasksByProjectResult>>
    {
        public Guid ProjectId { get; set; }
    }
}
