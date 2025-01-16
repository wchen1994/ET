using DotRecast.Core;
using Unity.Mathematics;

namespace ET
{
    public static class MathHelper
    {
        public static bool ApproximatelyZero(this float3 vec)
        {
            return math.abs(vec.x) + math.abs(vec.y) + math.abs(vec.z) < math.EPSILON;
        }

        public static bool Approximately(this float3 a, float3 b)
        {
            return ApproximatelyZero(a - b);
        }

        public static RcVec3f Convert(this float3 value)
        {
            return new RcVec3f(-value.x, value.y, value.z);
        }
        
        public static float3 Convert(this RcVec3f value)
        {
            return new float3(-value.x, value.y, value.z);
        }
    }
}