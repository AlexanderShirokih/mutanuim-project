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
    }
}
