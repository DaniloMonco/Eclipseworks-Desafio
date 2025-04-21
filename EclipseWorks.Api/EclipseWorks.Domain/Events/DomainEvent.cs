using EclipseWorks.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Domain.Events
{
    public abstract class DomainEvent<T>
    {
        protected DomainEvent(T data, Guid userId)
        {
            TimeStamp = DateTime.Now;
            Data = data;
            UserId = userId;
        }

        public DateTime TimeStamp { get; private set; }
        public T Data { get; protected set; }
        public Guid UserId { get; private set; }
    }
}
