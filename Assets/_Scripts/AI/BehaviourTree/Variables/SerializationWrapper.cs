using System;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Variables
{
    [Serializable]
    public class SerializationWrapper
    {
        [Serializable]
        private class Wrapper { public object Value; }

        [SerializeField]
        public string TypeName;
        [SerializeField]
        public string JsonValue;

        public SerializationWrapper() { }

        public SerializationWrapper(object obj)
        {
            TypeName = obj.GetType().AssemblyQualifiedName;
            JsonValue = JsonUtility.ToJson(new Wrapper { Value = obj });
        }

        public object GetValue()
        {
            Type type = Type.GetType(TypeName);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(JsonValue);
            object rawValue = wrapper.Value;
            if (rawValue == null)
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }

                return null;
            }
            return Convert.ChangeType(rawValue, type);
        }

        public static SerializationWrapper CreateDefault(VariableType type)
        {
            object defaultVal = type switch
            {
                VariableType.Int => default(int),
                VariableType.Float => default(float),
                VariableType.Bool => default(bool),
                VariableType.String => string.Empty,
                VariableType.Vector2 => Vector2.zero,
                VariableType.Vector3 => Vector3.zero,
                VariableType.Quaternion => Quaternion.identity,
                VariableType.Color => Color.white,
                _ => null
            };
            return new SerializationWrapper(defaultVal);
        }
    }
}