using System.Collections.Generic;
using UnityEngine;

namespace Descent.Extensions.Math
{
    public static class Math3DUtility
    {
        public static Vector3 NearestWorldAxis(Vector3 vector)
        {
            List<Vector3> worldVectors = new List<Vector3>() { Vector3.up, Vector3.down, Vector3.left,
                    Vector3.right, Vector3.back, Vector3.forward };
            float dotProduct = -Mathf.Infinity;
            Vector3 worldVectorToAlignWith = Vector3.zero;

            foreach (Vector3 worldVector in worldVectors)
            {
                float tempDotProduct = Vector3.Dot(vector, worldVector);
                if (tempDotProduct > dotProduct)
                {
                    dotProduct = tempDotProduct;
                    worldVectorToAlignWith = worldVector;
                }
            }

            return worldVectorToAlignWith;
        }
    }
}