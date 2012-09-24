using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")]
    public class when_configuring_a_request_reply_sender_channel_with_a_hook : WithSenderSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress)
                    .WithHook(The<IMessageProcessor<object, object>>())
            .Initialise();

        It should_build_the_recieve_channel_with_the_specified_hook = () =>
            The<IReplyRecieveChannelBuilder>().WasToldTo(b => 
                b.Build( 
                    GetEndpointAddress(ChannelName, Environment.MachineName), 
                    The<IMessageProcessor<object, object>>()));        
    }
}