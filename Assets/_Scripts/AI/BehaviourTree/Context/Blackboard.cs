using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Context
{
    public class Blackboard
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public void Set<T>(string key, T value)
        {
            _data[key] = value;
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            if (_data.TryGetValue(key, out var obj) && obj is T t)
            {
                return t;
            }

            return defaultValue;
        }

        public bool Has(string key)
        {
            return _data.ContainsKey(key);
        }

        public void Clear(string key)
        {
            _data.Remove(key);
        }
    }
}