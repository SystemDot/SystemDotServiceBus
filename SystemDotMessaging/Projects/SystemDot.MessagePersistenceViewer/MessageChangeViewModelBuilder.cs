using System;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage.Esent;
using SystemDot.Serialisation;

namespace SystemDot.MessagePersistenceViewer
{
    public class MessageChangeViewModelBuilder
    {
        readonly ISerialiser serialiser;
        readonly EndpointAddressBuilder addressBuilder;

        public MessageChangeViewModelBuilder(ISerialiser serialiser, EndpointAddressBuilder addressBuilder)
        {
            this.serialiser = serialiser;
            this.addressBuilder = addressBuilder;
        }

        public void Build(MainViewModel mainViewModel)
        {
            using (var store = new EsentChangeStore(this.serialiser))
            {
                store.Initialise(mainViewModel.DatabaseLocation);

                var changeRoot = new MessageChangeRoot(
                    store,
                    mainViewModel,
                    this.addressBuilder.Build(mainViewModel.ChannelName, Environment.MachineName),
                    mainViewModel.PersistenceUseType);

                mainViewModel.Clear();
                changeRoot.Initialise();
            }
        }
    }
}