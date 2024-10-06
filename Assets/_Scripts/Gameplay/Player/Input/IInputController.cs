namespace ProjectSC.Gameplay.Player.Input
{
    public interface IInputController
    {
        float LookX { get; }
        float LookY { get; }

        float MoveX { get; }
        float MoveY { get; }

        void Refresh();
    }
}