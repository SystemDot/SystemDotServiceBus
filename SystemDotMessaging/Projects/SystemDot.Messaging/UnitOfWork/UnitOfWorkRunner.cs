using System;
using System.Diagnostics.Contracts;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.UnitOfWork
{
    class UnitOfWorkRunner : IMessageProcessor<object, object>
    {
        readonly IUnitOfWorkFactory unitOfWorkFactory;
        readonly ISerialiser serialiser;

        public UnitOfWorkRunner(IUnitOfWorkFactory unitOfWorkFactory, ISerialiser serialiser)
        {
            Contract.Requires(unitOfWorkFactory != null);
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.serialiser = serialiser;
        }

        public void InputMessage(object toInput)
        {
            var unitOfWork = this.unitOfWorkFactory.Create();

            unitOfWork.Begin();

            try
            {
                MessageProcessed(toInput);
            }
            catch (Exception exception)
            {
                unitOfWork.End(exception);
                throw new UnitOfWorkException(serialiser.SerialiseToString(toInput), exception);
            }

            unitOfWork.End();
        }

        public event Action<object> MessageProcessed;
    }
}