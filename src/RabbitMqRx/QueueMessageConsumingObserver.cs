using System;
using System.Reactive;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqRx
{
    public class QueueMessageConsumingObserver : IObserver<ConsumingRequisites>
    {
        private readonly IModel _model;
        
        private bool _isBlocked = false;

        public QueueMessageConsumingObserver(IModel model)
        {
            _model = model;
        }

        public void OnCompleted()
        {
            _isBlocked = true;
        }

        public void OnError(Exception error)
        {
            _isBlocked = true;
        }

        public void OnNext(ConsumingRequisites value)
        {
            if (_isBlocked)
            {
                return;
            }
            
            switch (value.Result)
            {
                case ConsumingResult.Acknowledge:
                    _model.BasicAck(value.DeliveryTag, false);
                    break;
                
                case ConsumingResult.RejectAndRequeue:
                    _model.BasicReject(value.DeliveryTag, true);
                    break;
                
                case ConsumingResult.RejectAndDiscard:
                    _model.BasicReject(value.DeliveryTag, false);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}