using System;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;

namespace SystemDot.Messaging.UnitOfWork
{
    class UnitOfWorkRunner<TUnitOfWorkFactory> : IMessageProcessor<object, object>
        where TUnitOfWorkFactory : class, IUnitOfWorkFactory
    {
        readonly IIocContainer container;

        public UnitOfWorkRunner(IIocContainer container)
        {
            Contract.Requires(container != null);
            this.container = container;
        }

        public void InputMessage(object toInput)
        {
            var unitOfWork = this.container.Resolve<TUnitOfWorkFactory>()
                .As<TUnitOfWorkFactory>()
                .Create();

            unitOfWork.Begin();

            try
            {
                this.MessageProcessed(toInput);
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