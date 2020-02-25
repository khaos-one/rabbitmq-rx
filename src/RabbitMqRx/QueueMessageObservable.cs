using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqRx
{
    public sealed class QueueMessageObservable : IObservable<QueueMessage>
    {
        private readonly IModel _model;
        private readonly EventingBasicConsumer _consumer;
        private readonly List<IObserver<QueueMessage>> _observers;

        public QueueMessageObservable(IModel model, string queueName, ushort prefetchCount = 1)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException(nameof(queueName));
            }

            _model.BasicQos(0, prefetchCount, false);
            _model.BasicQos(0, prefetchCount, true);
            
            _consumer = new EventingBasicConsumer(model);
            _consumer.Received += ConsumerOnReceived;
            _consumer.Shutdown += ConsumerOnShutdown;
            _consumer.ConsumerCancelled += ConsumerOnConsumerCancelled;
            
            _observers = new List<IObserver<QueueMessage>>();

            _model.BasicConsume(
                _consumer, queueName, autoAck: false);
        }

        private void ConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new QueueMessage(e));
            }
        }
        
        private void ConsumerOnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }
        
        private void ConsumerOnShutdown(object sender, ShutdownEventArgs e)
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<QueueMessage> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<QueueMessage>(_observers, observer);
        }
    }
}