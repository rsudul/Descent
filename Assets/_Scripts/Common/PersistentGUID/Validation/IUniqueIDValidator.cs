using System;
using Descent.Common.PersistentGUID.Validation.Data;

namespace Descent.Common.PersistentGUID.Validation
{
    public interface IUniqueIDValidator
    {
        event Action<ValidationResult> OnValidationCompleted;
    }
}