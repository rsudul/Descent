using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Variables
{
    [System.Serializable]
    public class VariableDefinition
    {
        [SerializeField]
        private string _guid = Guid.NewGuid().ToString();
        public string GUID => _guid;

        [SerializeField]
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, "^[A-Za-z_][A-Za-z0-9_]*$"))
                {
                    throw new ArgumentException("Invalid variable name.");
                }

                _name = value;
            }
        }

        [SerializeField]
        public VariableType VariableType;
        [SerializeField]
        public string EnumTypeName = string.Empty;

        [SerializeField]
        private SerializationWrapper _defaultValue;
        public object DefaultValue
        {
            get => _defaultValue.GetValue();
            set => _defaultValue = new SerializationWrapper(value);
        }

        [SerializeField]
        public float? MinValue;
        [SerializeField]
        public float? MaxValue;
        [SerializeField]
        public string Description;

        public VariableDefinition(string name, VariableType type)
        {
            Name = name;
            VariableType = type;
            _defaultValue = SerializationWrapper.CreateDefault(type);
        }
    }
}