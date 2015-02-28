using EventStore.ClientAPI.SystemData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace IdeaSoftware.EventStore.Client.Core
{
    public class Config
    {
        public static Config Load(string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;

            var settings = connectionString.TrimEnd(';').Split(';').ToDictionary(setting => setting.Split('=')[0].Trim(), setting => setting.Split('=')[1].Trim());

            IPAddress ip;
            int tcpPort, httpPort;

            Validate(settings, out ip, out tcpPort, out httpPort);

            return new Config
            {
                TcpEndpoint = new IPEndPoint(ip, tcpPort),
                HttpEndpoint = new IPEndPoint(ip, httpPort),
                Credentials = new UserCredentials(settings["user"], settings["password"])
            };
        }

        private static void Validate(Dictionary<string, string> settings, out IPAddress ip, out int tcpPort, out int httpPort)
        {
            if (!settings.ContainsKey("ip"))
                throw new ConfigurationErrorsException(String.Format("Connection String does not contain field ip."));
            if (!settings.ContainsKey("http-port"))
                throw new ConfigurationErrorsException(String.Format("Connection String does not contain field http-port."));
            if (!settings.ContainsKey("tcp-port"))
                throw new ConfigurationErrorsException(String.Format("Connection String does not contain field tcp-port."));
            if (!settings.ContainsKey("user"))
                throw new ConfigurationErrorsException(String.Format("Connection String does not contain field user."));
            if (!settings.ContainsKey("password"))
                throw new ConfigurationErrorsException(String.Format("Connection String does not contain field password."));
            if (!IPAddress.TryParse(settings["ip"], out ip))
                throw new FormatException(String.Format("Ip {0} is Invalid", settings["ip"]));
            if (!Int32.TryParse(settings["tcp-port"], out tcpPort))
                throw new FormatException(String.Format("TCP port {0} is Invalid", settings["tcp-port"]));
            if (tcpPort < 1024 || tcpPort > 49151)
                throw new FormatException(String.Format("TCP port {0} is out of range", settings["tcp-port"]));
            if (!Int32.TryParse(settings["http-port"], out httpPort))
                throw new FormatException(String.Format("HTTP port {0} is Invalid", settings["http-port"]));
            if (httpPort < 1024 || httpPort > 49151)
                throw new FormatException(String.Format("HTTP port {0} is out of range", settings["http-port"]));
        }


        public UserCredentials Credentials { get; set; }
        public IPEndPoint TcpEndpoint { get; set; }
        public IPEndPoint HttpEndpoint { get; set; }
    }
}
