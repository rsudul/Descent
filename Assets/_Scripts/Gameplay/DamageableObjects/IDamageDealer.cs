namespace Descent.Gameplay.DamageableObjects
{
    public interface IDamageDealer
    {
        public int Damage { get; }
        public DamageType DamageType { get; }
    }
}