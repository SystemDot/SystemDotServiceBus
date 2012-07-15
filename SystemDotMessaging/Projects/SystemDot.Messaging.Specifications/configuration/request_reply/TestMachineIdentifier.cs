namespace SystemDot.Messaging.Specifications.configuration.request_reply
{
    public class TestMachineIdentifier : IMachineIdentifier
    {
        readonly string machineName;

        public TestMachineIdentifier(string machineName)
        {
            this.machineName = machineName;
        }

        public string GetMachineName()
        {
            return this.machineName;
        }
    }
}