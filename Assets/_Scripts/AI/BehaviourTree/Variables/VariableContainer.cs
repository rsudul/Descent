using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Variables
{
    [Serializable]
    public class VariableContainer
    {
        [SerializeField]
        private List<VariableDefinition> _variables = new List<VariableDefinition>();
        private Dictionary<string, VariableDefinition> _lookup;

        public IReadOnlyList<VariableDefinition> Variables => _variables;

        public void AddVariable(VariableDefinition def)
        {
            if (_variables.Exists(v => v.Name == def.Name))
            {
                throw new ArgumentException($"Variable with name '{def.Name}' already exists.");
            }

            _variables.Add(def);
            UpdateLookup();
        }

        public void RemoveVariable(string guid)
        {
            VariableDefinition def = GetByGUID(guid);
            _variables.Remove(def);
            UpdateLookup();
        }

        public void ClearVariables()
        {
            _variables.Clear();
        }

        private void UpdateLookup()
        {
            _lookup = _variables.ToDictionary(v => v.GUID, v => v);
        }

        public VariableDefinition GetByGUID(string guid)
        {
            UpdateLookup();

            if (!_lookup.TryGetValue(guid, out VariableDefinition def))
            {
                throw new KeyNotFoundException($"Variable GUID '{guid}' not found.");
            }

            return def;
        }

        public VariableDefinition GetByName(string name)
        {
            VariableDefinition def = _variables.FirstOrDefault(v => v.Name == name);
            if (def == null)
            {
                throw new KeyNotFoundException($"Variable name '{name}' not found.");
            }

            return def;
        }
    }
}