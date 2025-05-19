using System.Collections.Generic;

namespace Descent.Common.PersistentGUID.Validation
{
    public interface IUniqueIDAutoFixer
    {
        void FixMissingIDs(IEnumerable<GeneratedUniqueID> missingIDs);
        void FixDuplicateIDs(IEnumerable<GeneratedUniqueID> duplicateIDs);
    }
}