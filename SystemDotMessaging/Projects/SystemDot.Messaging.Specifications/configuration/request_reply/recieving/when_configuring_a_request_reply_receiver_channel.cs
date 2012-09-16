using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_receiver_channel : WithRecieverSubject
    {
        const string ChannelName = "Test";
        static IBus bus;

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
            .OpenChannel(ChannelName)
            .ForRequestReplyRecieving()
            .Initialise();

        It should_build_the_request_recieve_channel = () =>
            The<IRequestRecieveChannelBuilder>().WasToldTo(
                b => b.Build(GetEndpointAddress(ChannelName, Environment.MachineName)));

        private It should_build_the_reply_send_channel = () => 
            The<IReplySendChannelBuilder>().WasToldTo(
                b => b.Build(GetEndpointAddress(ChannelName, Environment.MachineName)));

        It should_register_the_listening_address_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => 
                r.StartPolling(GetEndpointAddress(ChannelName, Environment.MachineName)));

        It should_build_the_acknowledgement_channel = () => The<IAcknowledgementChannelBuilder>().WasToldTo(b => b.Build());

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
        
    }
}