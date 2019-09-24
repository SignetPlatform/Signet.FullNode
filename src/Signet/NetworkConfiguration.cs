using System.Linq;

namespace Signet
{
   public class NetworkConfiguration
   {
      public string Identifier { get; set; }

      public string Chain { get; set; }

      public string Name { get; set; }

      public int Port { get; set; }

      public int RpcPort { get; set; }

      public int ApiPort { get; set; }

      public int WsPort { get; set; }
   }

   public class NetworkConfigurations
   {
      private readonly NetworkConfiguration[] networks;

      public NetworkConfigurations()
      {
         this.networks = new NetworkConfiguration[] {
                new NetworkConfiguration() {
                    Identifier = "main",
                    Chain = "signet",
                    Name = "Signet Main",
                    Port = 4341,
                    RpcPort = 4342,
                    ApiPort = 4343,
                    WsPort = 4344
                },

                new NetworkConfiguration() {
                    Identifier = "regtest",
                    Chain = "signet",
                    Name = "Signet RegTest",
                    Port = 14341,
                    RpcPort = 14342,
                    ApiPort = 14343,
                    WsPort = 14344
                },

                new NetworkConfiguration() {
                    Identifier = "testnet",
                    Chain = "signet",
                    Name = "Signet Test",
                    Port = 24341 ,
                    RpcPort = 24342,
                    ApiPort = 24343,
                    WsPort = 24344
                },

                 new NetworkConfiguration() {
                    Identifier = "main",
                    Chain = "stratis",
                    Name = "Stratis Main",
                    Port = 16178,
                    RpcPort = 16174,
                    ApiPort = 37221,
                    WsPort = 4336
                },

                 new NetworkConfiguration() {
                    Identifier = "testnet",
                    Chain = "stratis",
                    Name = "Stratis Test",
                    Port = 26178,
                    RpcPort = 26174,
                    ApiPort = 38221,
                    WsPort = 4336
                },
            };
      }

      public NetworkConfiguration[] GetNetworks()
      {
         return this.networks;
      }

      public NetworkConfiguration GetNetwork(string identifier, string chain)
      {
         return this.networks.FirstOrDefault(n => n.Identifier == identifier && n.Chain == chain);
      }
   }
}