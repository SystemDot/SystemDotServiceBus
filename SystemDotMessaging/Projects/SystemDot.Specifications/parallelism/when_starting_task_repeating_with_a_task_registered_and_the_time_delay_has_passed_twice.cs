using System;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.parallelism
{
    [Subject("parallelism")]
    public class when_starting_task_repeating_with_a_task_registered_and_the_time_delay_has_passed_twice
        : WithSubject<TaskRepeater>
    {
        static TestTaskScheduler scheduler;
        static int taskHappened;
        static TimeSpan delay;
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);

            scheduler = new TestTaskScheduler(systemTime)
            {
                SchedulesPermitted = 2
            };

            Configure<ITaskScheduler>(scheduler);

            delay = TimeSpan.FromSeconds(1);
            Subject.Register(delay, () => taskHappened++);
        };

        Because of = () =>
        {
            Subject.Start();
            systemTime.AdvanceTime(TimeSpan.FromSeconds(1));
            systemTime.AdvanceTime(TimeSpan.FromSeconds(1));
        };

        It should_repeat_the_task_twice = () => taskHappened.ShouldEqual(2);
    }
}