using Descent.Common.PersistentGUID.Validation.Data;

namespace Descent.Common.PersistentGUID.Validation
{
    public interface IUniqueIDSceneValidator : IUniqueIDValidator
    {
        ValidationResult ValidateScene(UnityEngine.SceneManagement.Scene scene);
    }
}