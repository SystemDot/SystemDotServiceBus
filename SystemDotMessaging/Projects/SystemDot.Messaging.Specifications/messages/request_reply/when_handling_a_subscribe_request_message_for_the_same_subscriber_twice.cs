using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using Machine.Specifications;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications.messages.request_reply
{
    [Subject("Message publishing")]
    public class when_handling_a_subscribe_request_message_for_the_same_subscriber_twice
        : WithMessageInputterSubject<SubscriptionRequestHandler>
    {
        static EndpointAddress address;
        static IMessageInputter<MessagePayload> subscriptionChannel;
        static MessagePayload request;
        static SubscriptionSchema subscriptionSchema;

        Establish context = () =>
        {
            address = new EndpointAddress("TestAddress", "TestServer");
            subscriptionChannel = new Pipe<MessagePayload>();
            subscriptionSchema = new SubscriptionSchema(new EndpointAddress("TestSubscriberAddress", "TestServer"));

            Configure<ISendChannelBuilder>(An<ISendChannelBuilder>());
            Configure<IRecieveChannelBuilder>(An<IRecieveChannelBuilder>());
            
            request = new MessagePayload();
            request.SetToAddress(address);
            request.SetSubscriptionRequest(subscriptionSchema);
            Subject.InputMessage(request);
        };

        Because of = () => Subject.InputMessage(request);

        It should_setup_the_reply_channels_only_once = () =>
            The<ISendChannelBuilder>().WasToldTo(b => b.Build(subscriptionSchema.SubscriberAddress)).OnlyOnce();

        It should_setup_the_request_channels_only_once = () =>
            The<IRecieveChannelBuilder>().WasToldTo(b => b.Build()).OnlyOnce();
    }
}