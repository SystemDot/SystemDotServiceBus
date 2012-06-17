using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.Configuration.Channels;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Messaging.Configuration.Remote;
using SystemDot.Serialisation;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration
{
    public class Configure
    {
        const int DefaultWorkerThreads = 4;

        static Configure()
        {
            MessagingEnvironment.SetComponent<IThreadPool>(new ThreadPool(DefaultWorkerThreads));
            MessagingEnvironment.SetComponent<IThreader>(new Threader());
            MessagingEnvironment.SetComponent(new ThreadedWorkCoordinator(MessagingEnvironment.GetComponent<IThreader>()));
            MessagingEnvironment.SetComponent<IWebRequestor>(new WebRequestor());
            MessagingEnvironment.SetComponent<IFormatter>(new BinaryFormatter());
            MessagingEnvironment.SetComponent<ISerialiser>(new BinarySerialiser(MessagingEnvironment.GetComponent<IFormatter>()));
        }
        
        public static RemoteConfiguration Remote()
        {
            return new RemoteConfiguration();
        }

        public static LocalConfiguration Local()
        {
            return new LocalConfiguration();
        }        
    }
}