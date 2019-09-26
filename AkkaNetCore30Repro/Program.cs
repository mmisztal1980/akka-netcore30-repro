using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Configuration;

namespace AkkaNetCore30Repro
{
    internal class Program
    {
        private static ActorSystem _system;

        private static void Main(string[] args)
        {
            string[] hocons =
            {
                "akka : {\r\n    stdout-loglevel : INFO\r\n    loglevel : INFO\r\n    log-config-on-start : on\r\n    loggers : [\"Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog\"]\r\n    actor : {\r\n      debug : {\r\n        receive : on\r\n        autoreceive : on\r\n        lifecycle : on\r\n        event-stream : on\r\n        unhandled : on\r\n      }\r\n    }\r\n  }",
                "akka : {\r\n    remote : {\r\n      dot-netty : {\r\n        tcp : {\r\n          log-transport : true\r\n          transport-class : \"Akka.Remote.Transport.DotNetty.TcpTransport, Akka.Remote\"\r\n          transport-protocol : tcp\r\n          hostname : 0.0.0.0\r\n          public-hostname : localhost\r\n          port : 9000\r\n        }\r\n      }\r\n    }\r\n  }",
                "akka : {\r\n    actor : {\r\n      provider : \"Akka.Cluster.ClusterActorRefProvider, Akka.Cluster\"\r\n    }\r\n    cluster : {\r\n      log-info : on\r\n      seed-nodes : [\"akka.tcp://System@localhost:9000\"]\r\n      roles : []\r\n      role : []\r\n    }\r\n  }"
            };

            var config = ConfigurationFactory.Empty;

            foreach (var hocon in hocons)
            {
                config = config.WithFallback(Akka.Configuration.ConfigurationFactory.ParseString(hocon));
            }

            // NullReferenceException
            _system = ActorSystem.Create("System", config);

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}