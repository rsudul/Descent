namespace Descent.Gameplay.Health
{
    public interface IRepairable
    {
        void RepairFull();
        void RepairPartial(float amount);
    }
}