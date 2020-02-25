using System;
using System.Collections.Generic;

namespace RabbitMqRx
{
    public class Unsubscriber<T> : IDisposable
    {
        private readonly ICollection<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public Unsubscriber(ICollection<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}