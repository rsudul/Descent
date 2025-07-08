namespace Descent.Gameplay.Systems.Durability.Repair
{
    public interface IRepairable
    {
        void RepairFull();
        void RepairPartial(float amount);
    }
}