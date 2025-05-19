namespace Descent.Common.PersistentGUID.Validation
{
    public interface IUniqueIDProjectValidator : IUniqueIDValidator
    {
        void ValidateProject(IUniqueIDSceneValidator sceneValidator);
    }
}