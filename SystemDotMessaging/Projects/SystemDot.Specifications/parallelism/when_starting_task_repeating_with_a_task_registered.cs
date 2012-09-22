using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.parallelism
{
    [Subject("parallelism")]
    public class when_starting_task_repeating_with_a_task_registered : WithSubject<TaskRepeater>
    {
        static TestTaskScheduler scheduler;
        static TimeSpan delay;
        static List<TimeSpan> delays;

        Establish context = () =>
        {
            scheduler = new TestTaskScheduler(3, new TestCurrentDateProvider(DateTime.Now));
            Configure<ITaskScheduler>(scheduler);

            delay = TimeSpan.FromSeconds(1);
            delays = new List<TimeSpan>();
            Subject.Register(delay, () => delays.Add(scheduler.LastDelay));
        };

        Because of = () => Subject.Start();

        It should_repeat_the_task = () => delays.Count.ShouldEqual(3);

        It should_run_the_task_straight_away = () => delays.First().ShouldEqual(TimeSpan.FromMilliseconds(1));

        It should_repeat_the_task_after_the_specified_time_delay = () => delays.ElementAt(1).ShouldEqual(delay);
    }
}