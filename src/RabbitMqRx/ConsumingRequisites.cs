namespace RabbitMqRx
{
    public class ConsumingRequisites
    {
        public ulong DeliveryTag { get; }
        public ConsumingResult Result { get; }

        public ConsumingRequisites(QueueMessage message, ConsumingResult result)
        {
            DeliveryTag = message.DeliveryTag;
            Result = result;
        }
    }
}