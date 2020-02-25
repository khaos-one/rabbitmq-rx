using RabbitMQ.Client.Events;

namespace RabbitMqRx
{
    public class QueueMessage : BasicDeliverEventArgs
    {
        public QueueMessage(BasicDeliverEventArgs e)
        {
            ConsumerTag = e.ConsumerTag;
            DeliveryTag = e.DeliveryTag;
            Redelivered = e.Redelivered;
            Exchange = e.Exchange;
            RoutingKey = e.RoutingKey;
            BasicProperties = e.BasicProperties;
            Body = e.Body;
        }
    }
}