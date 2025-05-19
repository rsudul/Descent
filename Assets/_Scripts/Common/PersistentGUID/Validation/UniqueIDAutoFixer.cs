using System.Collections.Generic;
using UnityEditor;

namespace Descent.Common.PersistentGUID.Validation
{
    public class UniqueIDAutoFixer : IUniqueIDAutoFixer
    {
        public void FixMissingIDs(IEnumerable<GeneratedUniqueID> missingIDs)
        {
            Undo.RecordObjects(missingIDs as UnityEngine.Object[], "Fix Missing Unique IDs");

            foreach (GeneratedUniqueID uid in missingIDs)
            {
                uid.AssignNewUniqueID();
            }
        }

        public void FixDuplicateIDs(IEnumerable<GeneratedUniqueID> duplicateIDs)
        {
            Undo.RecordObjects(duplicateIDs as UnityEngine.Object[], "Fix Duplicate Unique IDs");

            foreach (GeneratedUniqueID uid in duplicateIDs)
            {
                uid.AssignNewUniqueID();
            }
        }
    }
}