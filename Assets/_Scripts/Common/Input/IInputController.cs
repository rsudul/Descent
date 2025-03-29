namespace Descent.Common.Input
{
    public interface IInputController
    {
        float LookX { get; }
        float LookY { get; }
        float Banking { get; }

        float MoveX { get; }
        float MoveY { get; }

        bool Shoot { get; }
        bool SaveGame { get; }
        bool LoadGame { get; }

        void Refresh();
    }
}