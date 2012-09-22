using System;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.parallelism
{
    [Subject("parallelism")]
    public class when_starting_task_repeating_with_two_tasks_registered : WithSubject<TaskRepeater>
    {
        static bool repeated1;
        static bool repeated2;

        Establish context = () =>
        {
            Configure<ITaskScheduler>(new ZeroTimespanPassThroughTaskScheduler());

            Subject.Register(new TimeSpan(0, 0, 1), () => repeated1 = true);
            Subject.Register(new TimeSpan(0, 0, 1), () => repeated2 = true);
        };

        Because of = () => Subject.Start();

        It should_repeat_the_first_task = () => repeated1.ShouldBeTrue();

        It should_repeat_the_second_task = () => repeated2.ShouldBeTrue();        
    }
}