using System;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Channels.UnitOfWork
{
    public class UnitOfWorkRunner<TUnitOfWorkFactory> : IMessageProcessor<object, object>
        where TUnitOfWorkFactory : class, IUnitOfWorkFactory
    {
        readonly IIocContainer iocContainer;

        public UnitOfWorkRunner(IIocContainer iocContainer)
        {
            Contract.Requires(iocContainer != null);
            this.iocContainer = iocContainer;
        }

        public void InputMessage(object toInput)
        {
            var unitOfWork = this.iocContainer
                .Resolve<TUnitOfWorkFactory>()
                .Create();

            unitOfWork.Begin();

            try
            {
                MessageProcessed(toInput);
            }
            catch (Exception exception)
            {
                unitOfWork.End(exception);
                throw;
            }

            unitOfWork.End();
        }

        public event Action<object> MessageProcessed;
    }
}