using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Channels.Messages.Recieving;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.long_polling
{
    [Subject("Long polling")]
    public class when_sending_a_long_poll_request
    {
        static Address address; 
        static TestWebRequestor requestor;
        static BinaryFormatter formatter;
        static LongPollReciever reciever;
        
        Establish context = () =>
        {
            formatter = new BinaryFormatter();
            requestor = new TestWebRequestor();
            requestor.ResponseStream.Serialise(new List<MessagePayload>(), formatter);

            address = new Address("Address");
            reciever = new LongPollReciever(address, requestor, formatter);
        };

        Because of = () => reciever.PerformWork();

        It should_send_the_long_poll_request = () =>
            requestor.RequestStream.Deserialise<MessagePayload>(formatter)
                .IsLongPollRequest().ShouldBeTrue();

        It should_send_the_long_poll_request_with_the_correct_address = () =>
            requestor.RequestStream.Deserialise<MessagePayload>(formatter).Address.ShouldEqual(address);

        It should_send_the_long_poll_request_to_the_correct_address = () =>
            requestor.AddressUsed.ShouldEqual(address.Url);
    }
}