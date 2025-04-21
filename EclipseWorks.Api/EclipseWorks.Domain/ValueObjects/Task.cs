using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.ValueObjects
{
    public class Task
    {
        
        protected Task(Guid referencekey, int priority, int status, string name, string comments, DateTime updateAt, Guid projectId)
        {
            ReferenceKey = referencekey;
            Status = (StatusEnum) status;
            Priority =  (PriorityEnum) priority;
            Name = name;
            Comments = comments;
            UpdateAt = updateAt;
            ProjectId = projectId;
        }
        
        public Task(PriorityEnum priority, string name, string comments, Guid projectId)
        {
            ReferenceKey = Guid.NewGuid();
            Status = StatusEnum.Created;
            Name = name;
            Priority = priority;
            Comments = comments;
            ProjectId = projectId;
        }
        //Um Objeto de Valor não tem um "Id" por definição. Para não confundir, utilizei outra nomenclatura para 
        //a chave primaria no banco de dados e para identificar um tarefa dentro da listagem do projeto
        public Guid ReferenceKey { get; private set; }
        public PriorityEnum Priority { get; private set; }
        public StatusEnum Status { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Comments { get; private set; } = string.Empty;
        public DateTime? UpdateAt { get; private set; }
        public Guid ProjectId { get; private set; }

        public void ChangeComments(string comments)
        {
            Comments = comments;
        }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeStatus(StatusEnum status)
        {
            Status = status;
            if (status == StatusEnum.Done)
                UpdateAt = DateTime.Now;
            else
                UpdateAt = null;
        }
    }


}
