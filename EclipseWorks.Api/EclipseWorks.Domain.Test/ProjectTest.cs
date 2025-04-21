using EclipseWorks.Domain.Entities;
using EclipseWorks.Domain.Exceptions;

namespace EclipseWorks.Domain.Test
{
    public class ProjectTest
    {
        [Fact]
        public void Give_me_success_when_project_has_less_then_20_tasks()
        {
            var project = new Project("project 1", "description 1", Guid.NewGuid());
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 1", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 2", string.Empty, project.Id));

            Assert.Equal(2, project.Tasks.Count);
            Assert.Equal("project 1", project.Name);
            Assert.Equal("description 1", project.Description);
        }


        [Fact]
        public void Give_me_exception_when_project_has_more_then_20_tasks()
        {
            var project = new Project("project 1", "description 1", Guid.NewGuid());
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 1", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 2", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 3", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 4", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 5", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 6", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 7", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 8", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 9", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 10", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 11", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 12", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 13", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 14", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 15", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 16", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 17", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 18", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 19", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 20", string.Empty, project.Id));

            var act = () => project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 21", string.Empty, project.Id)); ;

            DomainException exception = Assert.Throws<DomainException>(act);
            
            Assert.Equal("The project has a maximum limit of 20 tasks", exception.Message);
        }

        [Fact]
        public void Give_me_sucess_when_all_tasks_are_done()
        {
            var project = new Project("project 1", "description 1", Guid.NewGuid());

            var task1 = new ValueObjects.Task(PriorityEnum.High, "Task 1", string.Empty, project.Id);
            var task2 = new ValueObjects.Task(PriorityEnum.High, "Task 2", string.Empty, project.Id);
            task1.ChangeStatus(StatusEnum.Done);
            task2.ChangeStatus(StatusEnum.Done);
            project.AddTask(task1);
            project.AddTask(task2);

            Assert.True(project.CanBeRemoved());
        }

        [Fact]
        public void Give_me_false_when_all_tasks_are_done()
        {
            var project = new Project("project 1", "description 1", Guid.NewGuid());

            var task1 = new ValueObjects.Task(PriorityEnum.High, "Task 1", string.Empty, project.Id);
            var task2 = new ValueObjects.Task(PriorityEnum.High, "Task 2", string.Empty, project.Id);
            task2.ChangeStatus(StatusEnum.Done);
            project.AddTask(task1);
            project.AddTask(task2);

            Assert.False(project.CanBeRemoved());
        }

        [Fact]
        public void Give_me_success_when_remove_task()
        {
            var project = new Project("project 1", "description 1", Guid.NewGuid());
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 1", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 2", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 3", string.Empty, project.Id));

            project.RemoveTask(project.Tasks[1].ReferenceKey);

            Assert.Equal(2, project.Tasks.Count);
            Assert.Equal("Task 1", project.Tasks[0].Name);
            Assert.Equal("Task 3", project.Tasks[1].Name);
        }

        [Fact]
        public void Give_me_exception_when_cannot_find_task_remove()
        {
            var project = new Project("project 1", "description 1", Guid.NewGuid());
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 1", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 2", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 3", string.Empty, project.Id));

            var act = () => project.ChangeTask(Guid.NewGuid(), project.Tasks[1].Name, project.Tasks[1].Comments, project.Tasks[1].Status); ;

            DomainException exception = Assert.Throws<DomainException>(act);
            Assert.Equal("Could not find task referenceKey", exception.Message);
        }


        [Fact]
        public void Give_me_exception_when_cannot_find_task_change()
        {
            var project = new Project("project 1", "description 1", Guid.NewGuid());
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 1", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 2", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 3", string.Empty, project.Id));

            var act = () => project.ChangeTask(Guid.NewGuid(), project.Tasks[1].Name, project.Tasks[1].Comments, project.Tasks[1].Status); ;

            DomainException exception = Assert.Throws<DomainException>(act);
            Assert.Equal("Could not find task referenceKey", exception.Message);
        }

        [Fact]
        public void Give_me_success_when_change_task()
        {
            var project = new Project("project 1", "description 1", Guid.NewGuid());
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 1", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 2", string.Empty, project.Id));
            project.AddTask(new ValueObjects.Task(PriorityEnum.High, "Task 3", string.Empty, project.Id));

            project.ChangeTask(project.Tasks[1].ReferenceKey, "Changed", project.Tasks[1].Comments, StatusEnum.Done);


            Assert.Equal(3, project.Tasks.Count);
            Assert.Equal("Task 1", project.Tasks[0].Name);
            Assert.Equal(StatusEnum.Created, project.Tasks[0].Status);
            Assert.Equal("Changed", project.Tasks[1].Name);
            Assert.Equal(StatusEnum.Done, project.Tasks[1].Status);
            Assert.Equal("Task 3", project.Tasks[2].Name);
            Assert.Equal(StatusEnum.Created, project.Tasks[2].Status);
        }
    }
}