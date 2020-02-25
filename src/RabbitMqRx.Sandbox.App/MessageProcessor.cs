using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace RabbitMqRx.Sandbox.App
{
    public class MessageProcessor : ISubject<QueueMessage, ConsumingRequisites>
    {
        private readonly List<IObserver<ConsumingRequisites>> _observers = new List<IObserver<ConsumingRequisites>>();
        
        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted!");

            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("OnError!");

            foreach (var observer in _observers)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(QueueMessage value)
        {
            Console.WriteLine($"OnNext: {value.DeliveryTag} — {Encoding.UTF8.GetString(value.Body)}");

            foreach (var observer in _observers)
            {
                observer.OnNext(new ConsumingRequisites(value, ConsumingResult.Acknowledge));
            }
        }

        public IDisposable Subscribe(IObserver<ConsumingRequisites> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<ConsumingRequisites>(_observers, observer);
        }
    }
}