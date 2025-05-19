using System.Collections.Generic;

namespace Descent.Common.PersistentGUID.Validation.Data
{
    public class ValidationResult
    {
        public List<GeneratedUniqueID> MissingIDs { get; }
        public List<GeneratedUniqueID> DuplicateIDs { get; }

        public ValidationResult(List<GeneratedUniqueID> missingIDs, List<GeneratedUniqueID> duplicateIDs)
        {
            MissingIDs = missingIDs;
            DuplicateIDs = duplicateIDs;
        }
    }

}