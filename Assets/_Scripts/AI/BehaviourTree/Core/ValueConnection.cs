namespace Descent.AI.BehaviourTree.Core
{
    [System.Serializable]
    public class ValueConnection
    {
        public string SourceNodeGUID;
        public string SourcePinName;
        public string TargetNodeGUID;
        public string TargetPinName;
    }
}