using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Messages.Processing.RequestReply;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.processing.filtering
{
    [Subject("Message filtering")]
    public class when_filtering_a_message_for_reply_where_the_last_request_matches : WithSubject<MessageFilter>
    {
        static object message;
        static object processed;
        static EndpointAddress recieverAddress;

        Establish context = () =>
        {
            recieverAddress = new EndpointAddress("Channel", "Server");

            The<ReplyAddressLookup>().SetCurrentRecieverAddress(recieverAddress);
            Subject = new MessageFilter(new ReplyChannelMessageFilterStategy(The<ReplyAddressLookup>(), recieverAddress));

            Subject.MessageProcessed += i => processed = i;
            message = new object();
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processed.ShouldEqual(message);
        
    }

    [Subject("Message filtering")]
    public class when_filtering_a_message_for_reply_where_the_last_request_does_not_match : WithSubject<MessageFilter>
    {
        static object message;
        static object processed;

        Establish context = () =>
        {
            The<ReplyAddressLookup>().SetCurrentRecieverAddress(new EndpointAddress("Channel", "Server"));
            
            Subject = new MessageFilter(new ReplyChannelMessageFilterStategy(The<ReplyAddressLookup>(), new EndpointAddress("OtherChannel", "OtherServer")));

            Subject.MessageProcessed += i => processed = i;
            message = new object();
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_pass_the_message_through = () => processed.ShouldBeNull();

    }
}