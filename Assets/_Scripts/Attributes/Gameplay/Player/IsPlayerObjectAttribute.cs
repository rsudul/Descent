using System;

namespace Descent.Attributes.Gameplay.Player
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IsPlayerObjectAttribute : Attribute
    {
        public IsPlayerObjectAttribute() { }
    }
}