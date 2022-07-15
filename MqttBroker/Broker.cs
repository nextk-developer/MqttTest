using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace MqttBroker
{
    internal class Broker
    {
        static async Task Main(string[] args)
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                    .WithConnectionBacklog(100)
                    .WithDefaultEndpointPort(8222);

            IMqttServer broker = new MqttFactory().CreateMqttServer();

            broker.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e => OnMessageReceived(e));
            broker.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(e => OnClientConnected(e));
            broker.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(e => OnClientDisconnected(e));
            broker.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandlerDelegate(e => OnClientSubscribedToTopic(e));
            broker.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(e => OnClientUnsubscribedToTopic(e));

            await broker.StartAsync(optionsBuilder.Build());    

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static void OnClientUnsubscribedToTopic(MqttServerClientUnsubscribedTopicEventArgs e)
        {
            Console.WriteLine($"OnClientUnsubscribedToTopic: {e.ClientId} - {e.TopicFilter}");
        }

        private static void OnClientSubscribedToTopic(MqttServerClientSubscribedTopicEventArgs e)
        {
            Console.WriteLine($"OnClientSubscribedToTopic: {e.ClientId} - {e.TopicFilter.Topic}");
        }

        private static void OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($"OnMessageReceived: {e.ApplicationMessage.Topic}, {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            MetaData data = JsonSerializer.Deserialize<MetaData>(e.ApplicationMessage.Payload);
        }

        private static void OnClientConnected(MqttServerClientConnectedEventArgs e)
        {
            Console.WriteLine($"OnClientConnected: {e.ClientId}");
        }

        private static void OnClientDisconnected(MqttServerClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"OnClientDisconnected: {e.ClientId} - {e.DisconnectType}");
        }
    }
}
