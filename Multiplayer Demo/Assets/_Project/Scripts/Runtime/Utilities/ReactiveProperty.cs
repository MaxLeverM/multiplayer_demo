using System;
using System.Collections.Generic;

namespace Gameplay
{
    public class ReactiveProperty<T>
    {
        private T _value;
        private readonly List<Action<T>> _listeners;
        
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyListeners();
            }
        }
        
        public ReactiveProperty(T initValue)
        {
            _listeners = new List<Action<T>>();
            Value = initValue;
        }

        public void AddListener(Action<T> listener)
        {
            _listeners.Add(listener);
            listener?.Invoke(_value);
        }

        public void RemoveListener(Action<T> listener)
        {
            _listeners.Remove(listener);
        }

        private void NotifyListeners()
        {
            foreach (var listener in _listeners)
            {
                listener?.Invoke(_value);
            }
        }
    }
}