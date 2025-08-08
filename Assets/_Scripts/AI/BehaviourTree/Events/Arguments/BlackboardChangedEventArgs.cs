using System;

namespace Descent.AI.BehaviourTree.Events.Arguments
{
    public class BlackboardChangedEventArgs : EventArgs
    {
        string _key = string.Empty;
        object _value;

        public BlackboardChangedEventArgs(string key, object value)
        {
            _key = key;
            _value = value;
        }
    }
}