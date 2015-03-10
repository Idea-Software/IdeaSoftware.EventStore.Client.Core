using System;
using System.Configuration;
using NUnit.Framework;
using IdeaSoftware.EventStore.Client.Core;

namespace IdeaSoftware.EventStore.Client.Core.Tests
{
    [TestFixture]
    public class ConfigShould
    {
        [TestCase("NoIp")]
        [TestCase("NoPort")]
        [TestCase("NoUser")]
        [TestCase("NoPassword")]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ThrowException_WhenRequiredFieldMissing(string name)
        {
            Config.Load(name);
        }


        [TestCase("InvalidIp1")]
        [TestCase("InvalidIp2")]
        [TestCase("InvalidIp3")]
        [TestCase("InvalidIp4")]
        [ExpectedException(typeof(FormatException))]
        public void ThrowException_WhenIpInvalid(string name)
        {
            Config.Load(name);
        }

        [TestCase("InvalidPort1")]
        [TestCase("InvalidPort2")]
        [TestCase("InvalidPort3")]
        [TestCase("InvalidPort4")]
        [ExpectedException(typeof(FormatException))]
        public void ThrowException_WhenPortInvalid(string name)
        {
            Config.Load(name);
        }
    }
}
