using EclipseWorks.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Domain.Repository
{
    public interface IRepository<T> where T : IAggregateRoot
    {

    }
}
