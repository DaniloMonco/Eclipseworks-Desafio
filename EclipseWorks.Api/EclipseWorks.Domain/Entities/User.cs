using EclipseWorks.Domain.Aggregates;
using EclipseWorks.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EclipseWorks.Domain.Entities
{
    public class User :  IAggregateRoot
    {
        protected User(Guid id, UserPositionEnum position)
        {
            Id= id;
            Position= position;
        }
        public User(UserPositionEnum position)
        {
            Id = Guid.NewGuid();
            Position = position;
        }

        public Guid Id { get; private set; }
        public UserPositionEnum Position { get; private set; }

        public bool IsManager() => Position == UserPositionEnum.Manager;
    }
}
