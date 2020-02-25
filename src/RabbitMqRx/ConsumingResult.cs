namespace RabbitMqRx
{
    public enum ConsumingResult
    {
        Acknowledge,
        RejectAndRequeue,
        RejectAndDiscard
    }
}