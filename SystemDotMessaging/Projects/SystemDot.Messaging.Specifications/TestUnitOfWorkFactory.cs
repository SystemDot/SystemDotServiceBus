using SystemDot.Messaging.Channels.UnitOfWork;

namespace SystemDot.Messaging.Specifications
{
    public class TestUnitOfWorkFactory : IUnitOfWorkFactory
    {
        readonly TestUnitOfWork unitOfWork;

        public TestUnitOfWorkFactory(TestUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IUnitOfWork Create()
        {
            return this.unitOfWork;
        }
    }
}