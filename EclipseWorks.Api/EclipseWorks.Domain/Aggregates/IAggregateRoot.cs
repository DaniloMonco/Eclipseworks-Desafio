using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Domain.Aggregates
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
