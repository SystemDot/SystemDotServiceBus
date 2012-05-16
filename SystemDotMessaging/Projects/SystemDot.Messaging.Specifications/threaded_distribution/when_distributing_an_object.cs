using SystemDot.Messaging.Threading;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.threaded_distribution
{
    public class when_distributing_an_object
    {
        static TestThreader threader; 
        static ThreadedDistributor distributor;
        static object toDistribute;
        static object distributed;

        Establish context = () =>
        {
            toDistribute = new object();
            threader = new TestThreader();
            distributor = new ThreadedDistributor(2, threader);
            distributor.Start(o => distributed = o);
        };

        Because of = () =>
        {
            distributor.Distribute(toDistribute);
            distributor.Stop();
            threader.RunStarts();
        };

        It should_distribute_the_object_to_the_performer_action = () => distributed.ShouldBeTheSameAs(toDistribute);
    }
}