using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.UnitOfWork
{
    class UnitOfWorkRunner : IMessageProcessor<object, object>
    {
        readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UnitOfWorkRunner(IUnitOfWorkFactory unitOfWorkFactory)
        {
            Contract.Requires(unitOfWorkFactory != null);
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void InputMessage(object toInput)
        {
            var unitOfWork = this.unitOfWorkFactory.Create();

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