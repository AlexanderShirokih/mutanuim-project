using UnityEngine;

namespace Mutanium
{
    namespace Utils
    {
        public static class RandomUtils
        {
            public static bool GetRandomBool()
            {
                return Random.Range(0, 2) == 0;
            }
        }

        public static class MeasurementUtils
        {
            public static float GetModelRadius(GameObject game)
            {
                Bounds b = game.GetComponent<Renderer>().bounds;
                Vector3 size = b.size;
                return Mathf.Max(size.x, size.z);
            }
        }
    }
}
