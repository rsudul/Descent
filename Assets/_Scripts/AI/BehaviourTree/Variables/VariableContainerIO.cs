using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Variables
{
    public static class VariableContainerIO
    {
        public static void ExportToJson(VariableContainer variableContainer, string path)
        {
            VariableDefinitionListWrapper wrapper = new VariableDefinitionListWrapper
            {
                Variables = new List<VariableDefinition>(variableContainer.Variables)
            };

            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(path, json);
        }

        public static void ImportFromJson(VariableContainer variableContainer, string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            string json = File.ReadAllText(path);
            VariableDefinitionListWrapper wrapper = JsonUtility.FromJson<VariableDefinitionListWrapper>(json);

            variableContainer.ClearVariables();
            foreach (VariableDefinition variableDefinition in wrapper.Variables)
            {
                variableContainer.AddVariable(variableDefinition);
            }
        }
    }
}