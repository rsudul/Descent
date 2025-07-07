namespace Descent.Gameplay.Systems.Health
{
    public interface IRepairable
    {
        void RepairFull();
        void RepairPartial(float amount);
    }
}