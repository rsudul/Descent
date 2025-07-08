namespace Descent.Gameplay.Systems.Durability.Damage
{
    public interface IDamageDealer
    {
        public int Damage { get; }
        public DamageType DamageType { get; }
    }
}