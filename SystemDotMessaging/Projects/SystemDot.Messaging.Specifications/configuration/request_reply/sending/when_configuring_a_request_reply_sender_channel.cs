using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_sender_channel : WithSenderSubject
    {
        const string ChannelName = "Test";
        private const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
            .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress)
            .Initialise();

        It should_build_the_send_channel_with_the_default_pass_through_message_filter = () =>
            The<IRequestSendChannelBuilder>().As<TestRequestSendChannelBuilder>().MessageFilter
                .ShouldBeOfType<PassThroughMessageFilterStategy>();

        It should_build_the_send_channel_with_the_correct_from_address = () =>
            The<IRequestSendChannelBuilder>().As<TestRequestSendChannelBuilder>().From.ShouldEqual(
                GetEndpointAddress(ChannelName, Environment.MachineName));

        It should_build_the_send_channel_with_the_correct_reciever_address = () =>
            The<IRequestSendChannelBuilder>().As<TestRequestSendChannelBuilder>().Reciever.ShouldEqual(
                GetEndpointAddress(RecieverAddress, Environment.MachineName));

        It should_build_the_recieve_channel = () =>
            The<IReplyRecieveChannelBuilder>().WasToldTo(b => 
                b.Build(
                    GetEndpointAddress(ChannelName, Environment.MachineName), 
                    new IMessageProcessor<object, object>[0]));

        It should_register_the_listening_address_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r =>
                r.StartPolling(GetEndpointAddress(ChannelName, Environment.MachineName)));

        It should_start_the_task_repeater = () => The<ITaskRepeater>().WasToldTo(r => r.Start());

        It should_intitialise_the_persistence_layer = () => The<IPersistence>().WasToldTo(p => p.Initialise());

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
    }
}