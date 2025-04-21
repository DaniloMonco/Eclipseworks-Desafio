using EclipseWorks.Domain.Aggregates;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Domain.Entities
{
    public class Project : IAggregateRoot
    {
        protected Project(Guid id, string name, string description, Guid userid)
        {
            Id= id;
            Name= name;
            Description= description;
            UserId = userid;
            Tasks = new List<ValueObjects.Task>();
        }
        public Project(string name, string? description, Guid userId) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Tasks = new List<ValueObjects.Task>();
            UserId = userId;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public IList<ValueObjects.Task> Tasks { get; private set; }

        public Guid UserId { get; private set; }

        public ValueObjects.Task AddTask(ValueObjects.Task task)
        {
            if (Tasks.Count >= 20)
                throw new DomainException("The project has a maximum limit of 20 tasks");

            Tasks.Add(task);
            return task;
        }


        public bool CanBeRemoved() => !Tasks.Any(task => task.Status != StatusEnum.Done);

        public ValueObjects.Task ChangeTask(Guid referenceKey, string name, string comments, StatusEnum status)
        {
            var task = Tasks.Where(t => t.ReferenceKey == referenceKey).FirstOrDefault();
            if (task == null)
                throw new DomainException("Could not find task referenceKey");

            task.ChangeStatus(status);
            task.ChangeName(name);
            task.ChangeComments(comments);
            return task;
        }

        public ValueObjects.Task RemoveTask(Guid referenceKey)
        {
            var task = Tasks.FirstOrDefault(t => t.ReferenceKey == referenceKey);
            if (task == null)
                throw new DomainException("Could not find task referenceKey");


            Tasks.Remove(task);
            return task;
        }
    }


}
