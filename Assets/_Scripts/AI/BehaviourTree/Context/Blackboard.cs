using Descent.AI.BehaviourTree.Events.Arguments;
using System;
using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Context
{
    public class Blackboard
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public event EventHandler<BlackboardChangedEventArgs> OnChanged;

        public void Set<T>(string key, T value)
        {
            _data[key] = value;

            BlackboardChangedEventArgs args = new BlackboardChangedEventArgs(key, value);
            OnChanged?.Invoke(this, args);
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (_data.TryGetValue(key, out var obj) && obj is T t)
            {
                value = t;
                return true;
            }

            value = default;
            return false;
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            return TryGet<T>(key, out var v) ? v : defaultValue;
        }

        public bool Has(string key)
        {
            return _data.ContainsKey(key);
        }

        public void Clear(string key)
        {
            if (_data.Remove(key))
            {
                BlackboardChangedEventArgs args = new BlackboardChangedEventArgs(key, null);
                OnChanged?.Invoke(this, args);
            }
        }
    }
}