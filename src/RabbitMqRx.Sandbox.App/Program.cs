using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMqRx.Sandbox.App
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "rabbitmq.development.svc.cluster.local",
                Port = 5672,
                UserName = "arbitro",
                Password = "yeisrkm"
            };

            using var connection = connectionFactory.CreateConnection();
            using var model = connection.CreateModel();

            var source = new QueueMessageObservable(model, "test");
            var processor = new MessageProcessor();
            var drain = new QueueMessageConsumingObserver(model);

            using var subscription1 = source.Subscribe(processor);
            using var subscription2 = processor.Subscribe(drain);

            await Console.In.ReadLineAsync();
        }
    }
}