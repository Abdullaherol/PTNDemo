using UnityEngine;

namespace Utility
{
    public static class VertexUtility
    {
        public static Vector3Int ConvertToVector3Int(this Vector3 vector)
        {
            return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
        }
    }
}