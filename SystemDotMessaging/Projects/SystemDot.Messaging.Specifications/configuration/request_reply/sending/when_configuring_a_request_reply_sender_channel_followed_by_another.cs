using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Configuration.HttpMessaging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_sender_channel_followed_by_another : WithConfiguationSubject
    {
        const string Channel1Name = "Channel1";
        const string Reciever1Address = "RecieverAddress1";
        const string Channel2Name = "Channel2";
        const string Reciever2Address = "RecieverAddress2";
        
        Establish context = () =>
        {
            Components.Registration = () =>
            {
                ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
                ConfigureAndRegister(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                ConfigureAndRegister<IRequestSendChannelBuilder>();
                ConfigureAndRegister<IRequestRecieveChannelBuilder>();
                ConfigureAndRegister<IReplyRecieveChannelBuilder>();
                ConfigureAndRegister<IMessageReciever>();
                ConfigureAndRegister<ITaskLooper>();
                ConfigureAndRegister<IBus>();
            };
        };

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(Channel2Name).ForRequestReplySendingTo(Reciever2Address)
                .OpenChannel(Channel2Name).ForRequestReplySendingTo(Reciever2Address)
            .Initialise();

        It should_build_the_second_send_channel = () =>
            The<IRequestSendChannelBuilder>().WasToldTo(b =>
                b.Build( 
                    GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName()),
                    GetEndpointAddress(Reciever2Address, The<IMachineIdentifier>().GetMachineName())));

        It should_register_the_listening_address_with_the_message_reciever_for_the_second_channel = () =>
            The<IMessageReciever>().WasToldTo(r =>
                r.RegisterListeningAddress(GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName())));
        
    }
}