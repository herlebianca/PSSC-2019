using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyPlanner.Models.DDD;
using MyPlanner.Repository;
using Moq;
using NUnit.Framework;

namespace MyPlanner.Tests
{
    public class MyTaskDDDTest
    {
        [Test]
        [Category("pass")]
        public void BasicFunctionsShouldWork()
        {
            var my_task = MyTaskFactory.Instance.CreateTask("test_description", DateTime.Today, "test_project", "test_owner", "test_asignee", MyTask.Convert("InProgress"), "test_review");

            my_task.AssignTask(new Volunteer("Bianca"));
            my_task.ChangeStatus(MyTask.Convert("Done"));
            my_task.GiveReview(new PlainText("very well"));

            Assert.AreEqual("Bianca", my_task.Asignee.ToString());
            Assert.AreEqual(MyTask.Convert("Done"), my_task.Status);
            Assert.AreEqual("very well", my_task.Review.ToString());
        }

        [Test]
        [Category("pass")]
        public void EventSpyShouldWork()
        {
            var logSpy = new Moq.Mock<IEventLogger>();
            var my_task = MyTaskFactory.Instance.CreateTask("test_description", DateTime.Today, "test_project", "test_owner", "test_asignee", MyTask.Convert("InProgress"), "test_review",logSpy.Object);
            var repository = new MyTaskRepository();
            repository.AddTask(my_task);

            repository.UpdateTask("AssignTask", my_task, "Bianca");
            repository.UpdateTask("ChangeStatus", my_task, "Done");
            repository.UpdateTask("GiveReview", my_task, "Very well");

            logSpy.Verify(_ => _.Log("Project : test_project Task : test_description was assigned to : Bianca"), Times.Once);
            logSpy.Verify(_ => _.Log("Project : test_project Task : test_description status has changed to : Done"), Times.Once);
            logSpy.Verify(_ => _.Log("Project : test_project Task : test_description has received a review : Very well"), Times.Once);
        }

        [Test]
        [Category("pass")]
        [TestCase(7,"2020-09-01")] //Today - ON
        [TestCase(3, "2020-12-01")] // IN
        [TestCase(1, "2020-14-01")] //One day before - ON
        public void DaysLeftAsOfShouldWork(int a, string b)
        {
            var logMock = new Moq.Mock<IEventLogger>();
            var my_task = MyTaskFactory.Instance.CreateTask("test_description", DateTime.Parse("2020-15-01"), "test_project", "test_owner", "test_asignee", MyTask.Convert("InProgress"), "test_review", logMock.Object);
            TimeSpan ts = new TimeSpan(a, 0, 0, 0);
            Assert.AreEqual(ts, my_task.DaysLeftAsOf(DateTime.Parse(b)));            
        }

        [Test]
        [Category("fail")]
        public void DaysLeftAsOfShouldFail()
        {
            var logMock = new Moq.Mock<IEventLogger>();
            var my_task = MyTaskFactory.Instance.CreateTask("test_description", DateTime.Parse("2020-15-01"), "test_project", "test_owner", "test_asignee", MyTask.Convert("InProgress"), "test_review", logMock.Object);
            Assert.That(() => my_task.DaysLeftAsOf(DateTime.Parse("2020-16-01")), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [Category("fail")]
        public void AddTaskShouldFail()
        {
            var my_task = MyTaskFactory.Instance.CreateTask("test_description", DateTime.Today, "test_project", "test_owner", "test_asignee", MyTask.Convert("InProgress"), "test_review");
            var repository = new MyTaskRepository();
            repository.AddTask(my_task);

            Assert.That(() => repository.AddTask(my_task), Throws.TypeOf<DuplicateWaitObjectException>());
        }
    }
}
