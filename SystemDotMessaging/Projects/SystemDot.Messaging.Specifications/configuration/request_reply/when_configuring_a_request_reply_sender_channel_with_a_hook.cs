using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Configuration.HttpMessaging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_sender_channel_with_a_hook : WithConfiguationSubject
    {
        const string ChannelName = "Test";
        private const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;
        
        Establish context = () =>
        {
            Components.Registration = () =>
            {
                ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
                ConfigureAndRegister(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                ConfigureAndRegister<ISendChannelBuilder>();
                ConfigureAndRegister<IReplyRecieveChannelBuilder>();
                ConfigureAndRegister<ISubscriptionRequestorChannelBuilder>();
                ConfigureAndRegister<IMessageReciever>();
                ConfigureAndRegister<ITaskLooper>();
                ConfigureAndRegister<IBus>();

                The<ISubscriptionRequestorChannelBuilder>()
                    .WhenToldTo(b => b.Build(
                        GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName()),
                        GetEndpointAddress(RecieverAddress, The<IMachineIdentifier>().GetMachineName())))
                    .Return(The<ISubscriptionRequestor>());
            };
        };

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(ChannelName).ForRequestReplySending(RecieverAddress)
                    .WithHook(The<IMessageProcessor<object, object>>())
            .Initialise();

        It should_build_the_recieve_channel_with_the_specified_hook = () =>
            The<IReplyRecieveChannelBuilder>().WasToldTo(b => b.Build(The<IMessageProcessor<object, object>>()));        
    }
}