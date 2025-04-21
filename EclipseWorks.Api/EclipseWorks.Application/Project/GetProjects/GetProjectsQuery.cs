using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Project.GetProjects
{
    public class GetProjectsQuery : IRequest<IEnumerable<GetProjectsResult>>
    {
        public Guid UserId { get; set; }
    }
}
