using System;
using System.Collections.Generic;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Hooks.Upgrading
{
    public class UpgradeMessageHook : IMessageHook<MessagePayload>
    {
        public static UpgradeMessageHook LoadUp()
        {
            return IocContainerLocator.Locate().Resolve<UpgradeMessageHook>();
        }

        readonly IList<IMessageUpgrader> upgraders;
        readonly ISerialiser serialiser;

        UpgradeMessageHook(ApplicationTypeActivator activator, ISerialiser serialiser)
        {
            upgraders = activator.InstantiateTypesOf<IMessageUpgrader>();
            this.serialiser = serialiser;
        }

        public void ProcessMessage(MessagePayload toInput, Action<MessagePayload> toPerformOnOutput)
        {
            toInput.SetBody(UpgradeBody(toInput.GetBody()));
            toPerformOnOutput(toInput);
        }

        byte[] UpgradeBody(byte[] toUpgrade)
        {
            upgraders.ForEach(upgrader => toUpgrade = UpgradeBody(upgrader, toUpgrade));
            return toUpgrade;
        }

        byte[] UpgradeBody(IMessageUpgrader upgrader, byte[] toUpgrade)
        {
            return upgrader
                .Upgrade(FromString(toUpgrade))
                .ToString()
                .ToBytes();
        }

        RawMessageBuilder FromString(byte[] toUpgrade)
        {
            return RawMessageBuilder.Parse(serialiser.DeserialiseToString(toUpgrade));
        }
    }
}