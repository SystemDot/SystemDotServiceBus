using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Errors.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Storage.Sql;

namespace SystemDot.Messaging.Utilities.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
                .UsingHttpTransport(MessageServer.Local())
                .UsingSqlPersistence(args[0]);
            
            if (args[1] == "r")
            {
                ReplayErrors();
                Console.ReadKey();
                return;
            }

            var persistenceUseType = PersistenceUseType.SubscriberSend;

            switch(args[2])
            {
                case "e":
                    persistenceUseType = PersistenceUseType.Error;
                    break;
                case "ps":
                    persistenceUseType = PersistenceUseType.PublisherSend;
                    break;
                case "rr":
                    persistenceUseType = PersistenceUseType.ReplyReceive;
                    break;
                case "rs":
                    persistenceUseType = PersistenceUseType.ReplySend;
                    break;
                case "rqr":
                    persistenceUseType = PersistenceUseType.RequestReceive;
                    break;
                case "rqs":
                    persistenceUseType = PersistenceUseType.RequestSend;
                    break;
                case "sr":
                    persistenceUseType = PersistenceUseType.SubscriberReceive;
                    break;
                case "srr":
                    persistenceUseType = PersistenceUseType.SubscriberRequestReceive;
                    break;
                case "srs":
                    persistenceUseType = PersistenceUseType.SubscriberRequestSend;
                    break;
            }

            string channel = persistenceUseType == PersistenceUseType.Error ? "errors" : args[3];

            var address = BuildEndpointAddress(channel, Environment.MachineName);

            var cache = Resolve<MessageCacheFactory>().CreateCache(persistenceUseType, address);

            Console.WriteLine("Listing messages for {0} channel {1}:", persistenceUseType, address);
            Console.WriteLine("-----------------------------------------------------------------");

            cache.GetMessages().ForEach(m => 
            {
                 Console.WriteLine("Id: {0}", m.Id);

                 if (m.HasToAddress())
                    Console.WriteLine("To: {0}", m.GetToAddress());  
                 else
                     Console.WriteLine("To: Local Channel");  

                 Console.WriteLine("--------------------------------------------------------------");   
            });

            Console.WriteLine("=====================================================================");
            Console.WriteLine("Current channel sequence: {0}", cache.GetSequence());
            
            Console.ReadKey();
        }

        static void ReplayErrors()
        {
            Resolve<ErrorResendChannelBuilder>().Build().ResendAllMessages();
        }

        protected static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        protected static EndpointAddress BuildEndpointAddress(string address, string defaultServerName)
        {
            return Resolve<EndpointAddressBuilder>().Build(address, defaultServerName);
        }
    }
}
