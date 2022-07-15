using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MqttClient
{
    internal class Client
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            IMqttClient mqttClient = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                            .WithClientId(Guid.NewGuid().ToString())
                            .WithTcpServer("127.0.0.1", 8222)
                            .WithCleanSession()
                            .Build();

            mqttClient.UseConnectedHandler(e =>
            {
                Console.WriteLine("Connected to the broker successfully");
            });

            mqttClient.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected from the broker successfully");
            });

            await mqttClient.ConnectAsync(options);

            Console.WriteLine("Please press a key to publish the message");

            Console.ReadLine();

            await PublishMessageAsync(mqttClient);

            await mqttClient.DisconnectAsync();
        }

        private static async Task PublishMessageAsync(IMqttClient client)
        {
            string messagePayload = JsonSerializer.Serialize(new MetaData());
            var message = new MqttApplicationMessageBuilder()
                            .WithTopic("NkMeta")
                            .WithPayload(messagePayload)
                            .WithAtLeastOnceQoS()
                            .Build();

            if (client.IsConnected)
            {
                await client.PublishAsync(message);
            }
        }
    }
}
