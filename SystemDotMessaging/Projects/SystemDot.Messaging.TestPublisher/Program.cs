using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.Channels.Building;
using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.Channels.Messages.Processing;
using SystemDot.Messaging.Channels.PubSub;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Serialisation;
using SystemDot.Threading;

namespace SystemDot.Messaging.TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            MessagingEnvironment.SetComponent<IThreadPool>(new ThreadPool(4));
            MessagingEnvironment.SetComponent<IThreader>(new Threader());
            MessagingEnvironment.SetComponent(new ThreadedWorkCoordinator(MessagingEnvironment.GetComponent<IThreader>()));
            MessagingEnvironment.SetComponent<IWebRequestor>(new WebRequestor());
            MessagingEnvironment.SetComponent<IFormatter>(new BinaryFormatter());
            MessagingEnvironment.SetComponent<ISerialiser>(new BinarySerialiser(MessagingEnvironment.GetComponent<IFormatter>()));
            MessagingEnvironment.SetComponent(new MessagePayloadCopier());
            MessagingEnvironment.SetComponent(new PublisherRegistry());
            
            ChannelBuilder
               .StartsWith(new MessageBus())
               .Pump()
               .ToProcessor(new MessagePayloadPackager(MessagingEnvironment.GetComponent<ISerialiser>()))
               .ThenToEndPoint(new Distributor(MessagingEnvironment.GetComponent<MessagePayloadCopier>()));

            do
            {
                Console.WriteLine("Press a key to send message..");
                Console.ReadLine();
                Console.WriteLine("Sending message");
                MessageBus.Send("Hello");
                MessageBus.Send("Hello1");
                MessageBus.Send("Hello2");
                MessageBus.Send("Hello3");
                MessageBus.Send("Hello4");
                MessageBus.Send("Hello5");
                MessageBus.Send("Hello6");
                MessageBus.Send("Hello7");
            }
            while (true);
        }
    }
}
