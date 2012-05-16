using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Recieving;
using SystemDot.Messaging.Channels.Recieving.Http;
using SystemDot.Messaging.Threading;

namespace SystemDot.Messaging.Configuration
{
    public class Configuration
    {
        readonly MessageListener listener;
        readonly List<ChannelConfiguration> channelConfigs;

        ThreadedDistributor threadedDistributor;
        
        public static Configuration ListeningOn(string address)
        {
            return new Configuration(new MessageListener(address));
        }

        Configuration(MessageListener listener)
        {
            this.listener = listener;
            this.channelConfigs = new List<ChannelConfiguration>();
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            this.channelConfigs.Add(new ChannelConfiguration(this, threadedDistributor, this.listener));
            return channelConfigs.Last();
        }

        public Configuration WorkerThreads(int threads)
        {
            threadedDistributor = new ThreadedDistributor(threads, new Threader());
            return this;
        }

        public Channel BuildSender()
        {
            return this.channelConfigs.First().Channel;
        }
    }
}