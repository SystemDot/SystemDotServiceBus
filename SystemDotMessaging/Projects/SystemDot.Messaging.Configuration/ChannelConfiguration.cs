using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Recieving;
using SystemDot.Messaging.Channels.Recieving.Http;
using SystemDot.Messaging.Channels.Sending;
using SystemDot.Messaging.Channels.Sending.Http;
using SystemDot.Messaging.Threading;

namespace SystemDot.Messaging.Configuration
{
    public class ChannelConfiguration
    {
        readonly Configuration configuration;
        readonly ThreadedDistributor threadedDistributor;
        readonly MessageListener listener;
        readonly List<ConsumerConfiguration> consumerConfigs;

        string deliveringToAddress;
        
        internal Channel Channel { get; private set; }

        public ChannelConfiguration(
            Configuration configuration, 
            ThreadedDistributor threadedDistributor, 
            MessageListener listener)
        {
            this.configuration = configuration;
            this.threadedDistributor = threadedDistributor;
            this.listener = listener;
            this.consumerConfigs = new List<ConsumerConfiguration>();
        }


        public ChannelConfiguration DeliveringTo(string address)
        {
            this.deliveringToAddress = address;
            return this;
        }

        public ConsumerConfiguration ConsumeMessages()
        {
            this.consumerConfigs.Add(new ConsumerConfiguration(this));
            return this.consumerConfigs.Last();
        }

        public Configuration Build()
        {
            Channel = new Channel(this.threadedDistributor);
            
            BuildPipeline();

            Channel.Start();

            return this.configuration;
        }

        void BuildPipeline()
        {
            if (this.deliveringToAddress != null)
            {
                BuildDeliveryPipeline();
            }

            if(this.consumerConfigs.Count > 0)
            {
                BuildConsumerPipeline();
            }
        }

        void BuildDeliveryPipeline()
        {
            var messageWebRequestor = new MessageWebRequestor(this.deliveringToAddress);
            new MessageSender(Channel, messageWebRequestor);
        }

        void BuildConsumerPipeline()
        {
            var messageReciever = new MessageReciever(Channel, this.listener);
            messageReciever.StartRecieving();

            var broadcaster = new ConsumerMessageBroadcaster(Channel);
            this.consumerConfigs.ForEach(c => broadcaster.RegisterConsumer(c.Consumer));
        }

    }
}