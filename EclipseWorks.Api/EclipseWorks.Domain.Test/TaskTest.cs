using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Test
{
    public class TaskTest
    {
        [Fact]
        public void Give_me_success_when_task_is_created()
        {
            var task = new ValueObjects.Task(PriorityEnum.High, "Task 1", "Task Comments", Guid.NewGuid());

            Assert.Equal(StatusEnum.Created, task.Status);
            Assert.Equal(PriorityEnum.High, task.Priority);
            Assert.Equal("Task 1", task.Name);
            Assert.Equal("Task Comments", task.Comments);
            Assert.NotEqual(Guid.Empty, task.ReferenceKey);
            Assert.Null(task.UpdateAt);
        }

        [Fact]
        public void Give_me_success_when_task_status_is_changed()
        {
            var task = new ValueObjects.Task(PriorityEnum.High, "Task 1", "Task Comments", Guid.NewGuid());
            task.ChangeStatus(StatusEnum.InProgress);

            Assert.Equal(StatusEnum.InProgress, task.Status);
            Assert.Equal(PriorityEnum.High, task.Priority);
            Assert.Equal("Task 1", task.Name);
            Assert.Equal("Task Comments", task.Comments);
            Assert.NotEqual(Guid.Empty, task.ReferenceKey);
            Assert.Null(task.UpdateAt);
        }

        [Fact]
        public void Give_me_success_when_task_status_is_done()
        {
            var task = new ValueObjects.Task(PriorityEnum.High, "Task 1", "Task Comments", Guid.NewGuid());
            task.ChangeStatus(StatusEnum.Done);

            Assert.Equal(StatusEnum.Done, task.Status);
            Assert.Equal(PriorityEnum.High, task.Priority);
            Assert.Equal("Task 1", task.Name);
            Assert.Equal("Task Comments", task.Comments);
            Assert.NotEqual(Guid.Empty, task.ReferenceKey);
            Assert.NotNull(task.UpdateAt);
        }

        [Fact]
        public void Give_me_success_when_task_status_is_undone()
        {
            var task = new ValueObjects.Task(PriorityEnum.High, "Task 1", "Task Comments", Guid.NewGuid());
            task.ChangeStatus(StatusEnum.Done);
            task.ChangeStatus(StatusEnum.InProgress);

            Assert.Equal(StatusEnum.InProgress, task.Status);
            Assert.Equal(PriorityEnum.High, task.Priority);
            Assert.Equal("Task 1", task.Name);
            Assert.Equal("Task Comments", task.Comments);
            Assert.NotEqual(Guid.Empty, task.ReferenceKey);
            Assert.Null(task.UpdateAt);
        }

        [Fact]
        public void Give_me_sucess_when_task_name_is_changed()
        {
            var task = new ValueObjects.Task(PriorityEnum.High, "Task 1", "Task Comments", Guid.NewGuid());
            task.ChangeName("Task changed");

            Assert.Equal(StatusEnum.Created, task.Status);
            Assert.Equal(PriorityEnum.High, task.Priority);
            Assert.Equal("Task changed", task.Name);
            Assert.Equal("Task Comments", task.Comments);
            Assert.NotEqual(Guid.Empty, task.ReferenceKey);
            Assert.Null(task.UpdateAt);
        }

        [Fact]
        public void Give_me_sucess_when_task_comments_is_changed()
        {
            var task = new ValueObjects.Task(PriorityEnum.High, "Task 1", "Task Comments", Guid.NewGuid());
            task.ChangeComments("Comments changed");

            Assert.Equal(StatusEnum.Created, task.Status);
            Assert.Equal(PriorityEnum.High, task.Priority);
            Assert.Equal("Task 1", task.Name);
            Assert.Equal("Comments changed", task.Comments);
            Assert.NotEqual(Guid.Empty, task.ReferenceKey);
            Assert.Null(task.UpdateAt);
        }
    }
}