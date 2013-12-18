using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class MessageServer : Equatable<MessageServer> 
    {
        public static MessageServer None { get { return new NullMessageServer(); } }

        public static MessageServer Named(string name, ServerAddress address)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(address != null);

            return new MessageServer(name, address);
        }

        public static MessageServer NamedMultipoint(string name, string machineName, ServerAddress address)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(!string.IsNullOrEmpty(machineName));
            Contract.Requires(address != null);

            return new MessageServer(name, machineName, address);
        }

        public string Name { get; set; }

        public string MachineName { get; set; }
       
        public ServerAddress Address { get; set; }

        public bool IsUnspecified{ get { return this is NullMessageServer; }}

        public MessageServer()
        {
        }

        MessageServer(string name, string machineName, ServerAddress address)  :this (name, address)
        {
            MachineName = machineName;
        }

        MessageServer(string name, ServerAddress address)
        {
            Name = name;
            Address = address;
        }

        public override bool Equals(MessageServer other)
        {
            return string.Equals(Name, other.Name) && other.MachineName == MachineName;
        }

        public override int GetHashCode()
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ (MachineName != null ? MachineName.GetHashCode() : 0);
            return hashCode;
        }

        public override string ToString()
        {
            return string.Concat(Name, GetMachineNameDescription(), " (", Address, ")");
        }

        string GetMachineNameDescription()
        {
            return MachineName != null ? string.Concat(".", MachineName) : string.Empty;
        }
    }
}