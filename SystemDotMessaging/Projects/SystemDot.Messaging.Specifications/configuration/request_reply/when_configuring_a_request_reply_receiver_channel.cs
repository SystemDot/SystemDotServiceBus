using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_receiver_channel : WithConfiguationSubject
    {
        const string ChannelName = "Test";
        static IBus bus;
        
        Establish context = () =>
        {
            Components.Registration = () =>
            {
                ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
                ConfigureAndRegister(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                ConfigureAndRegister<ISubscriptionRequestorChannelBuilder>();
                ConfigureAndRegister<IMessageReciever>();
                ConfigureAndRegister<ITaskLooper>();
                ConfigureAndRegister<IBus>();
            };
        };

        Because of = () => bus = Configuration.Configure
            .WithLocalMessageServer()
            .OpenChannel(ChannelName)
            .AsRequestReplyReciever()
            .Initialise();

        It should_build_the_subscription_request_channel_against_ = () => 
            The<ISubscriptionRequestorChannelBuilder>().WasToldTo(b => b.Build());

        It should_register_the_listening_address_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => 
                r.RegisterListeningAddress(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        It should_start_the_task_looper = () => The<ITaskLooper>().WasToldTo(l => l.Start());

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
        
    }
}